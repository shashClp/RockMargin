using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Windows.Input;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";

		IWpfTextView _view;
		MarksEnumerator _marks;
		ChangeEnumerator _changes;
		HighlightedWordsEnumerator _highlights;
		HighlightWordTagger _tagger;
		Track _track;
		TrackRender _render;


		public RockMargin(
			IWpfTextView view,
			MarksEnumerator marks,
			ChangeEnumerator changes,
			HighlightedWordsEnumerator highlights,
			HighlightWordTagger tagger)
		{
			_view = view;
			_marks = marks;
			_changes = changes;
			_highlights = highlights;
			_tagger = tagger;

			_track = new Track(view, this);
			_render = new TrackRender(_view, _track, _marks, _changes, _highlights);

			this.Width = _view.Options.GetOptionValue(OptionsKeys.Width);
			this.ClipToBounds = true;
			this.Background = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.BackgroundColor));

			view.Options.OptionChanged += OnOptionChanged;
			view.LayoutChanged += OnTextViewLayoutChanged;
			view.ZoomLevelChanged += OnViewStateChanged;
			highlights.WordsChanged += OnWordHighlightsChanged;
			marks.MarksChanged += OnMarksChanged;
			changes.ChangesChanged += OnChangesChanged;

			this.SizeChanged += OnViewStateChanged;
			this.MouseRightButtonDown += this.OnMouseRightButtonDown;
			this.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
			this.MouseLeftButtonUp += this.OnMouseLeftButtonUp;
			this.MouseMove += this.OnMouseMove;

			_render.Visuals.ForEach(delegate(Visual v) { AddVisualChild(v); });
		}

		protected override int VisualChildrenCount
		{
			get { return _render.Visuals.Count; }
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= _render.Visuals.Count)
				throw new ArgumentOutOfRangeException("index");

			return _render.Visuals[index];
		}

		private void OnOptionChanged(object sender, EditorOptionChangedEventArgs e)
		{
			if (e.OptionId == OptionsKeys.Width.Name)
			{
				this.Width = _view.Options.GetOptionValue(OptionsKeys.Width);
			}
			else if (e.OptionId == OptionsKeys.HighlightsEnabled.Name)
			{
				if (!_view.Options.GetOptionValue(OptionsKeys.HighlightsEnabled))
					_tagger.Clear();

				_render.Invalidate(TrackRender.MarginParts.WordHighlights);
			}
			else if (e.OptionId == OptionsKeys.MarginColor.Name)
			{
				_render.ReloadOptions();
				_render.Invalidate(TrackRender.MarginParts.Marks);
			}
			else if (e.OptionId == OptionsKeys.ScrollColor.Name || e.OptionId == OptionsKeys.ThumbColor.Name)
			{
				_render.ReloadOptions();
				_render.Invalidate(TrackRender.MarginParts.Scroll);
			}
			else if (e.OptionId == OptionsKeys.TextColor.Name || e.OptionId == OptionsKeys.CommentsColor.Name || e.OptionId == OptionsKeys.BackgroundColor.Name)
			{
				_render.ReloadOptions();
				_render.Invalidate(TrackRender.MarginParts.Text);

				this.Background = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.BackgroundColor));
			}
			else if (e.OptionId == OptionsKeys.HighlightColor.Name)
			{
				_render.ReloadOptions();
				_render.Invalidate(TrackRender.MarginParts.WordHighlights);
			}
			else if (e.OptionId == OptionsKeys.SavedChangeColor.Name || e.OptionId == OptionsKeys.UnsavedChangeColor.Name)
			{
				_render.ReloadOptions();
				_render.Invalidate(TrackRender.MarginParts.Changes);
			}
			else if (e.OptionId == OptionsKeys.ChangeMarginEnabled.Name)
			{
				_render.Invalidate(TrackRender.MarginParts.Changes | TrackRender.MarginParts.Text);
			}
			else if (e.OptionId == OptionsKeys.EnhancedTextRendering.Name)
			{
				_render.Invalidate(TrackRender.MarginParts.Text);
			}
		}

		private void OnWordHighlightsChanged(object source, EventArgs e)
		{
			_render.Invalidate(TrackRender.MarginParts.WordHighlights);
		}

		private void OnMarksChanged(object source, EventArgs e)
		{
			_render.Invalidate(TrackRender.MarginParts.Marks);
		}

		private void OnChangesChanged(object sender, EventArgs e)
		{
			_render.Invalidate(TrackRender.MarginParts.Changes);
		}

		private void OnTextViewLayoutChanged(object source, TextViewLayoutChangedEventArgs e)
		{
			if (e.NewViewState.EditSnapshot != e.OldViewState.EditSnapshot)
			{
				_marks.UpdateMarks();
				_changes.UpdateChanges();
				_render.Invalidate(TrackRender.MarginParts.All | TrackRender.MarginParts.Batched);
			}
			else if (e.NewViewState.VisualSnapshot != e.OldViewState.VisualSnapshot)
			{
				_marks.UpdateMarks();
				_changes.UpdateChanges();
				_render.Invalidate(TrackRender.MarginParts.All);
			}
			else
				_render.Invalidate(TrackRender.MarginParts.Scroll);
		}

		private void OnViewStateChanged(object source, EventArgs e)
		{
			_render.Invalidate(TrackRender.MarginParts.All);
		}

		void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			_tagger.Clear();
		}

		private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.CaptureMouse();

			Point p = e.GetPosition(this);
			_track.ScrollTo(p.Y);
			_render.Invalidate(TrackRender.MarginParts.Scroll);
		}

		private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.ReleaseMouseCapture();
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && this.IsMouseCaptured)
			{
				Point p = e.GetPosition(this);
				_track.ScrollTo(p.Y);
				_render.Invalidate(TrackRender.MarginParts.Scroll);
			}
		}

		public System.Windows.FrameworkElement VisualElement
		{
			get { return this; }
		}

		public double MarginSize
		{
			get { return this.ActualWidth; }
		}

		public bool Enabled
		{
			get { return true; }
		}

		public ITextViewMargin GetTextViewMargin(string marginName)
		{
			return (marginName == RockMargin.MarginName) ? this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
