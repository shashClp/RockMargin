using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;

namespace RockMargin
{
	struct HighlightedWord
	{
		public Span span;
		public int line;
	}

	class HighlightedWordsEnumerator
	{
		IWpfTextView _view;
		HighlightWordTagger _tagger;

		public event EventHandler<EventArgs> WordsChanged;

		public IEnumerable<HighlightedWord> Words
		{
			get { return GetWords(); }
		}

		public HighlightedWordsEnumerator(IWpfTextView view, HighlightWordTagger tagger)
		{
			_view = view;
			_tagger = tagger;
			_tagger.TagsChanged += OnTagsChanged;
		}

		private void OnTagsChanged(object source, SnapshotSpanEventArgs e)
		{
			WordsChanged(this, EventArgs.Empty);
		}

		private IEnumerable<HighlightedWord> GetWords()
		{
			ITextSnapshot snapshot = _view.TextSnapshot;

			var search_span = new SnapshotSpan(snapshot, 0, snapshot.Length);
			foreach (var tag in _tagger.GetTags(new NormalizedSnapshotSpanCollection(search_span)))
			{
				var spans = _view.BufferGraph.MapUpToSnapshot(tag.Span, SpanTrackingMode.EdgeExclusive, _view.VisualSnapshot);
				foreach (var span in spans)
				{
					HighlightedWord result;
					result.span = span;
					result.line = snapshot.GetLineNumberFromPosition(span.Start);

					yield return result;
				}
			}
		}
	}
}
