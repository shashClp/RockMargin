using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;

namespace RockMargin
{
	class HighlightWordTagger : ITagger<HighlightWordTag>
	{
		static Dictionary<IWpfTextView, HighlightWordTagger> _instances = new Dictionary<IWpfTextView, HighlightWordTagger>();

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		private IWpfTextView _view;
		private ITextSearchService _searcher;
		private ITextStructureNavigator _navigator;
		private IEditorFormatMap _format;
		private NormalizedSnapshotSpanCollection _spans = new NormalizedSnapshotSpanCollection();
		private SnapshotSpan? _word = null;
		private SnapshotPoint _point;
		private CaretPosition _caret;
		private object _lock = new object();


		public static HighlightWordTagger Instance(IWpfTextView view, IEditorFormatMap format, ITextSearchService searcher, ITextStructureNavigator navigator)
		{
			if (!_instances.ContainsKey(view))
				_instances[view] = new HighlightWordTagger(view, format, searcher, navigator);

			return _instances[view];
		}

		public HighlightWordTagger(IWpfTextView view, IEditorFormatMap format, ITextSearchService searcher, ITextStructureNavigator navigator)
		{
			_view = view;
			_searcher = searcher;
			_navigator = navigator;
			_format = format;

			view.VisualElement.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new RoutedEventHandler(ViewMouseLeftButtonDown), true);
			view.VisualElement.AddHandler(FrameworkElement.KeyUpEvent, new RoutedEventHandler(ViewKeyUp), true);
			
			view.Closed += OnViewClosed;
			view.Options.OptionChanged += OnOptionChanged;

			ReloadOptions();
		}

		private void ReloadOptions()
		{
			ResourceDictionary properties = _format.GetProperties(HighlightWordFormatDefinition.FormatName);

			Color forecolor = Utils.CreateColor(_view.Options.GetOptionValue(OptionsKeys.TextMarkerForegroundColor));
			Color backcolor = Utils.CreateColor(_view.Options.GetOptionValue(OptionsKeys.TextMarkerBackgroundColor));

			properties[HighlightWordFormatDefinition.ForegroundColorId] = forecolor;
			properties[HighlightWordFormatDefinition.BackgroundColorId] = backcolor;
		}

		private void OnOptionChanged(object sender, EditorOptionChangedEventArgs e)
		{
			if (e.OptionId == OptionsKeys.TextMarkerForegroundColor.Name || e.OptionId == OptionsKeys.TextMarkerBackgroundColor.Name)
			{
				ReloadOptions();
				FireTagsChanged();
			}
		}

		private void OnViewClosed(object sender, EventArgs e)
		{
			_instances.Remove(sender as IWpfTextView);
		}

		public void Clear()
		{
			_point = new SnapshotPoint();
			SynchronousUpdate(_point, new NormalizedSnapshotSpanCollection(), null);
		}

		private bool Enabled
		{
			get { return _view.Options.GetOptionValue(OptionsKeys.HighlightsEnabled);  }
		}

		private bool Active
		{
			get
			{
				return Keyboard.IsKeyDown(Key.LeftAlt) ||
					Keyboard.IsKeyDown(Key.RightAlt) ||
					!_view.Options.GetOptionValue(OptionsKeys.AltHighlights);
			}
		}

		void ViewKeyUp(object sender, RoutedEventArgs routed_e)
		{
			var e = routed_e as KeyEventArgs;
			if (e.Key == Key.Escape)
			{
				Clear();
			}
		}

		void ViewMouseLeftButtonDown(object sender, RoutedEventArgs routed_e)
		{
			if (Enabled && Active)
			{
				var e = routed_e as MouseButtonEventArgs;
				switch (e.ClickCount)
				{
					case 1:
						_caret = _view.Caret.Position;
						break;

					case 2:
						UpdateAtCaretPosition(_caret);
						break;
				}
			}
		}

		void UpdateAtCaretPosition(CaretPosition caretPosition)
		{
			SnapshotPoint? point = caretPosition.Point.GetPoint(_view.TextBuffer, caretPosition.Affinity);

			if (!point.HasValue)
				return;

			//	if the new caret position is still within the current word (and on the same snapshot), we don't need to check it
			if (_word.HasValue
				&& _word.Value.Snapshot == _view.TextSnapshot
				&& point.Value >= _word.Value.Start
				&& point.Value <= _word.Value.End)
			{
				return;
			}

			_point = point.Value;
			UpdateWordAdornments();
		}

		void UpdateWordAdornments()
		{
			SnapshotPoint current_request = _point;
			List<SnapshotSpan> word_spans = new List<SnapshotSpan>();

			//	find all words in the buffer like the one the caret is on
			TextExtent word = _navigator.GetExtentOfWord(current_request);
			bool word_found = true;

			//	If we've selected something not worth highlighting, we might have missed a "word" by a little bit
			if (!WordExtentIsValid(current_request, word))
			{
				//	before we retry, make sure it is worthwhile
				if (word.Span.Start != current_request
					 || current_request == current_request.GetContainingLine().Start
					 || char.IsWhiteSpace((current_request - 1).GetChar()))
				{
					word_found = false;
				}
				else
				{
					//	try again, one character previous
					//	if the caret is at the end of a word, pick up the word
					word = _navigator.GetExtentOfWord(current_request - 1);

					//	if the word still isn't valid, we're done
					if (!WordExtentIsValid(current_request, word))
						word_found = false;
				}
			}

			if (!word_found)
			{
				//	if we couldn't find a word, clear out the existing markers
				SynchronousUpdate(current_request, new NormalizedSnapshotSpanCollection(), null);
				return;
			}

			SnapshotSpan current_word = word.Span;
			//	if this is the current word, and the caret moved within a word, we're done.
			if (_word.HasValue && current_word == _word)
				return;

			//	find the new spans
			FindData find_data = new FindData(current_word.GetText(), current_word.Snapshot);
			find_data.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;

			word_spans.AddRange(_searcher.FindAll(find_data));

			//	if another change hasn't happened, do a real update
			if (current_request == _point)
				SynchronousUpdate(current_request, new NormalizedSnapshotSpanCollection(word_spans), current_word);
		}

		static bool WordExtentIsValid(SnapshotPoint current_request, TextExtent word)
		{
			return word.IsSignificant && current_request.Snapshot.GetText(word.Span).Any(c => char.IsLetter(c));
		}

		void SynchronousUpdate(SnapshotPoint current_request, NormalizedSnapshotSpanCollection new_spans, SnapshotSpan? new_word)
		{
			lock (_lock)
			{
				if (current_request != _point)
					return;

				_spans = new_spans;
				_word = new_word;

				FireTagsChanged();
			}
		}

		private void FireTagsChanged()
		{
			if (TagsChanged != null)
			{
				var snapshot = _view.TextSnapshot;
				TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, 0, snapshot.Length)));
			}
		}

		public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			if (_word == null)
				yield break;

			//	hold on to a "snapshot" of the word spans and current word, so that we maintain the same collection throughout
			SnapshotSpan current_word = _word.Value;
			NormalizedSnapshotSpanCollection word_spans = _spans;

			if (spans.Count == 0 || _spans.Count == 0)
				yield break;

			//	if the requested snapshot isn't the same as the one our words are on, translate our spans to the expected snapshot
			if (spans[0].Snapshot != word_spans[0].Snapshot)
			{
				word_spans = new NormalizedSnapshotSpanCollection(
					word_spans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

				current_word = current_word.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
			}

			// first, yield back the word the cursor is under (if it overlaps)
			// note that we'll yield back the same word again in the wordspans collection; the duplication here is expected
			if (spans.OverlapsWith(new NormalizedSnapshotSpanCollection(current_word)))
			{
				if (current_word.GetText() == _word.Value.GetText())
					yield return new TagSpan<HighlightWordTag>(current_word, new HighlightWordTag());
			}

			// second, yield all the other words in the file
			foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, word_spans))
			{
				if (span.GetText() == _word.Value.GetText())
					yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
			}
		}
	}
}
