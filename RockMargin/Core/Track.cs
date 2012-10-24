using System;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using Microsoft.VisualStudio.Text;


namespace RockMargin
{
	class Track
	{
		private IWpfTextView _view;
		private FrameworkElement _canvas;

		public Track(IWpfTextView view, FrameworkElement canvas)
		{
			_view = view;
			_canvas = canvas;
		}

		public double CanvasWidth
		{
			get { return _canvas.ActualWidth; }
		}

		public double CanvasHeight
		{
			get { return _canvas.ActualHeight; }
		}

		public double ThumbTop
		{
			get
			{
				var p = _view.BufferGraph.MapUpToBuffer(_view.TextViewLines.FirstVisibleLine.Start,
					PointTrackingMode.Negative, PositionAffinity.Predecessor, _view.VisualSnapshot.TextBuffer);

				int line_index = p.HasValue ? p.Value.GetContainingLine().LineNumber : 0;
				return line_index * LineHeight;
			}
		}

		public double ThumbBottom
		{
			get	{ return ThumbTop + ThumbHeight; }
		}

		public double ThumbHeight
		{
			get	{ return _view.ViewportHeight * LineHeight / _view.LineHeight; }
		}

		public double LineHeight
		{
			get
			{
				double height_in_lines = _view.VisualSnapshot.LineCount + _view.ViewportHeight / _view.LineHeight;
				return Math.Min(CanvasHeight / height_in_lines, 1.0);
			}
		}

		public double GetPositionFromLine(int line_number)
		{
			return line_number * LineHeight;
		}

		private int GetLineFromPosition(double y)
		{
			return (int)(y / LineHeight);
		}

		public void ScrollTo(double track_y)
		{
			var snapshot = _view.VisualSnapshot;

			int line_number = GetLineFromPosition(track_y - ThumbHeight / 2);
			line_number = Math.Max(line_number, 0);
			line_number = Math.Min(line_number, snapshot.LineCount - 1);

			var line = snapshot.GetLineFromLineNumber(line_number);
			var p = _view.BufferGraph.MapDownToBuffer(line.Start, PointTrackingMode.Negative, _view.TextBuffer, PositionAffinity.Predecessor);

			if (p.HasValue)
				_view.DisplayTextLineContainingBufferPosition(p.Value.GetContainingLine().Start, 0, ViewRelativePosition.Top);
		}
	}
}
