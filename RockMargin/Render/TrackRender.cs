using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using Microsoft.VisualStudio.Text.Classification;
using System.Threading;
using System.Windows.Threading;


namespace RockMargin
{
	class TrackRender
	{
		[Flags]
		public enum MarginParts
		{
			Scroll = 1,
			Marks = 2,
			WordHighlights = 4,
			Text = 8,
			All = Scroll | Marks | WordHighlights | Text,

			Batched = 128
		}

		const double MarksMarginWidth = 4;

		private Brush MarginBrush;
		private Brush ScrollBrush;
		private Brush ThumbBrush;
		private Brush HighlightBrush;
		private uint TextColor;
		private uint CommentsColor;
		private uint BackgroundColor;

		readonly IWpfTextView _view;
		readonly Track _track;
		readonly MarksEnumerator _marks;
		readonly HighlightedWordsEnumerator _highlights;

		public List<Visual> Visuals { get; private set; }
		private DrawingVisual _textVisual = new DrawingVisual();
		private DrawingVisual _marksVisual = new DrawingVisual();
		private DrawingVisual _scrollVisual = new DrawingVisual();
		private DrawingVisual _highlightsVisual = new DrawingVisual();
		private DrawingVisual _debugVisual = new DrawingVisual();

		static private int _stride = 0;
		static private object _lock = new object();
		static private WriteableBitmap _bufferBitmap = null;

		private DispatcherTimer _timer;
		private Thread _thread = null;
		private bool _invalidateText = false;


		Rect MarksMarginRectangle
		{
			get { return new Rect(0, 0, MarksMarginWidth, _track.CanvasHeight); }
		}

		Rect TopRectangle
		{
			get { return new Rect(MarksMarginWidth, 0, _track.CanvasWidth, _track.ThumbTop); }
		}

		Rect MiddleRectangle
		{
			get { return new Rect(MarksMarginWidth, _track.ThumbTop, _track.CanvasWidth, _track.ThumbBottom - _track.ThumbTop); }
		}

		Rect BottomRectangle
		{
			get { return new Rect(MarksMarginWidth, _track.ThumbBottom, _track.CanvasWidth, _track.CanvasHeight - _track.ThumbBottom); }
		}


		public TrackRender(IWpfTextView view, Track track, MarksEnumerator marks, HighlightedWordsEnumerator highlights)
		{
			_view = view;
			_track = track;
			_marks = marks;
			_highlights = highlights;

			_timer = new DispatcherTimer();
			_timer.Tick += OnInvalidateTextTimer;
			_timer.Interval = TimeSpan.FromMilliseconds(500);

			ReloadOptions();
			InitDrawingObjects();
		}

		private void OnInvalidateTextTimer(object sender, EventArgs e)
		{
			Invalidate(MarginParts.Text);
		}

		public void Invalidate(MarginParts parts)
		{
			if (parts.HasFlag(MarginParts.Scroll))
				InvalidateScroll();

			if (parts.HasFlag(MarginParts.Marks))
				InvalidateMarks();

			if (parts.HasFlag(MarginParts.WordHighlights))
				InvalidateHighlights();

			if (parts.HasFlag(MarginParts.Text))
			{
				if (parts.HasFlag(MarginParts.Batched))
				{
					if (!_invalidateText)	//	don't request text invalidation if we already have scheduled one
					{
						_timer.Stop();
						_timer.Start();
					}
				}
				else
				{
					InvalidateText();
				}
			}
		}

		public void ReloadOptions()
		{
			MarginBrush = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.MarginColor));
			ScrollBrush = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.ScrollColor));
			ThumbBrush = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.ThumbColor));
			HighlightBrush = Utils.CreateBrush(_view.Options.GetOptionValue(OptionsKeys.HighlightColor));
			TextColor = _view.Options.GetOptionValue(OptionsKeys.TextColor);
			CommentsColor = _view.Options.GetOptionValue(OptionsKeys.CommentsColor);
			BackgroundColor = _view.Options.GetOptionValue(OptionsKeys.BackgroundColor);
		}

		private void InitDrawingObjects()
		{
			Visuals = new List<Visual>();
			Visuals.Add(_textVisual);
			Visuals.Add(_marksVisual);
			Visuals.Add(_scrollVisual);
			Visuals.Add(_highlightsVisual);
			Visuals.Add(_debugVisual);

			//_textVisual.Effect = new Shader();
		}

		private void InvalidateScroll()
		{
			using (DrawingContext dc = _scrollVisual.RenderOpen())
			{
				DrawRectangle(dc, ScrollBrush, TopRectangle);
				DrawRectangle(dc, ThumbBrush, MiddleRectangle);
				DrawRectangle(dc, ScrollBrush, BottomRectangle);
			}
		}

		private void InvalidateMarks()
		{
			using (DrawingContext dc = _marksVisual.RenderOpen())
			{
				//	marks margin
				DrawRectangle(dc, MarginBrush, MarksMarginRectangle);

				//	marks
				foreach (var mark in _marks.Marks)
					DrawMark(dc, mark);
			}
		}

		private void InvalidateHighlights()
		{
			using (DrawingContext dc = _highlightsVisual.RenderOpen())
			{
				bool enabled = _view.Options.GetOptionValue(OptionsKeys.HighlightsEnabled);
				if (enabled)
				{
					foreach (var word in _highlights.Words)
						DrawHighlight(dc, word.span, word.line);
				}
			}
		}

		private void InvalidateDebug(long milliseconds)
		{
			using (DrawingContext dc = _debugVisual.RenderOpen())
			{
				DrawText(dc, milliseconds.ToString(), 10, 700);
			}
		}

		private Rect GetSegmentRectangle(Span span)
		{
			ITextSnapshotLine line = _view.VisualSnapshot.GetLineFromPosition(span.Start);

			double x = MarksMarginWidth + (span.Start - line.Start.Position);
			double y = Math.Floor(_track.GetPositionFromLine(line.LineNumber) + 0.5);
			double w = (span.Length);
			double h = 2.0;

			return new Rect(x, y, w, h);
		}

		private void DrawHighlight(DrawingContext dc, Span span, int line)
		{
			Rect rect = GetSegmentRectangle(span);

			if (rect.Right > _track.CanvasWidth)
				rect.Offset(_track.CanvasWidth - rect.Right, 0);

			DrawRectangle(dc, HighlightBrush, rect);
		}

		private void DrawMark(DrawingContext dc, TextMark mark)
		{
			double y = _track.GetPositionFromLine(mark.line) - _track.LineHeight / 2;

			Rect rect = new Rect(1, y, 1, 1);
			rect.Inflate(1.0, 1.0);

			DrawRectangle(dc, mark.brush, rect);
		}

		private void DrawRectangle(DrawingContext dc, Brush brush, Rect rect)
		{
			dc.DrawRectangle(brush, null, rect);
		}

		private void DrawText(DrawingContext dc, string text, double x, double y)
		{
			FormattedText format = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
				new Typeface("Verdana"), 14, Brushes.Red);

			format.MaxTextWidth = 300;
			format.MaxTextHeight = 240;

			dc.DrawText(format, new Point(x, y));
		}

		private WriteableBitmap CreateBufferBitmap(int width, int height)
		{
			if (_bufferBitmap == null || _bufferBitmap.Width < width || _bufferBitmap.Height < height)
			{
				//width = (width / 10 + 1) * 10;
				if (_bufferBitmap != null && _bufferBitmap.Width > width)
					width = _bufferBitmap.PixelWidth;

				height = (height / 10000 + 1) * 10000;
				if (_bufferBitmap != null && _bufferBitmap.Height > height)
					height = _bufferBitmap.PixelHeight;

				_bufferBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
				_stride = _bufferBitmap.BackBufferStride;
			}

			return _bufferBitmap;
		}

		private void InvalidateText()
		{
			if (_track.CanvasWidth > 0 && _track.CanvasHeight > 0)
			{
				_timer.Stop();
				if (_thread == null)
				{
					_thread = new Thread(new ThreadStart(DrawLinesAsync));
					_thread.Priority = ThreadPriority.Lowest;
					_thread.Start();
				}
				else //	deffer request execution
					_invalidateText = true;
			}
			else //	initial layouting is not complited yet
				Invalidate(MarginParts.Text | MarginParts.Batched);
		}

		private IntPtr LockBuffer(int width, int height)
		{
			CreateBufferBitmap(width, height);
			_bufferBitmap.Lock();

			return _bufferBitmap.BackBuffer;
		}

		private void UnlockBuffer(int width, int height, int dst_width, int dst_height)
		{
			_bufferBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
			_bufferBitmap.Unlock();

			if (_track.CanvasWidth > 0 && _track.CanvasHeight > 0)
			{
				ImageBrush text_brush = new ImageBrush(_bufferBitmap);
				text_brush.ViewboxUnits = BrushMappingMode.Absolute;
				text_brush.Viewbox = new Rect(0, 0, width, height);

				var drawing_group = new DrawingGroup();
				RenderOptions.SetBitmapScalingMode(drawing_group, BitmapScalingMode.HighQuality);
				var rectangle_geometry = new RectangleGeometry(new Rect(0, 0, dst_width, dst_height));
				var geometry_drawing = new GeometryDrawing(text_brush, null, rectangle_geometry);
				drawing_group.Children.Add(geometry_drawing);

				using (DrawingContext dc = _textVisual.RenderOpen())
				{
					dc.DrawDrawing(drawing_group);
				}
			
				var bmp = new RenderTargetBitmap(dst_width, (int)_track.CanvasHeight, 96, 96, PixelFormats.Pbgra32);
				bmp.Render(_textVisual);

				using (DrawingContext dc = _textVisual.RenderOpen())
				{
					dc.DrawImage(bmp, new Rect(MarksMarginWidth, 0, bmp.Width, bmp.Height));
				}
			}

			_thread = null;

			if (_invalidateText)
			{
				_invalidateText = false;
				InvalidateText();
			}
		}

		private void DrawLinesAsync()
		{
			Thread.CurrentThread.Name = "DrawLinesAsync";

			var lines = new List<ITextSnapshotLine>(_view.VisualSnapshot.Lines);
			var comments_tracker = CommentsTracker.Create(_view.TextBuffer.ContentType.TypeName);

			int width = (int)(_track.CanvasWidth - MarksMarginWidth);
			int height = lines.Count;

			int dst_width = width;
			int dst_height = (int)Math.Min(height, _track.CanvasHeight - _track.ThumbHeight);

			lock (_lock)
			{
				IntPtr pBackBuffer = (IntPtr)_textVisual.Dispatcher.Invoke(new Func<int, int, IntPtr>(LockBuffer), width, height);

				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				unsafe
				{
					int ptr = pBackBuffer.ToInt32();
					int stride = _stride;

					for (int row = 0; row < height; ++row)
					{
						string text = lines[row].GetText();
						for (int column = 0; column < Math.Max(text.Length, width); ++column)
						{
							bool is_comment = comments_tracker != null && column < text.Length && comments_tracker.TrackComment(text, column);

							if (column >= width)
								continue;

							// find the address of the pixel to draw
							int pPixelPointer = ptr;
							pPixelPointer += row * stride;
							pPixelPointer += column * 4;

							bool has_char = false;
							if (text != null && column < text.Length)
							{
								char c = text[column];
								has_char = !char.IsSeparator(c) && !char.IsControl(c);
							}

							// compute the pixel's color
							uint color_data = has_char ? (is_comment ? CommentsColor : TextColor) : BackgroundColor;

							// assign the color data to the pixel
							*((uint*)pPixelPointer) = color_data;

							/*if (row != 0 && column != 0 && color_data != BackgroundColor)
							{
								pPixelPointer -= stride;
								uint* a = (uint*)pPixelPointer;
								if (*a == BackgroundColor)
									*a = (color_data & 0x00ffffff) | 0x80000000;
							}*/
						}

						if (comments_tracker != null)
							comments_tracker.NewLine();
					}
				}

				//Thread.Sleep(3000);

#if DEBUG
				_textVisual.Dispatcher.Invoke(new Action<long>(InvalidateDebug), stopwatch.ElapsedMilliseconds);
#endif	

				_textVisual.Dispatcher.Invoke(new Action<int, int, int, int>(UnlockBuffer),	width, height, dst_width, dst_height);
			}
		}
	}
}