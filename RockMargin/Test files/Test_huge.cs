using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace RockMargin
{
	class RockMargin: Canvas, IWpfTextViewMargin
	{
		public const string MarginName = "RockMargin";
		private IWpfTextView _textView;
		private TextParser _textParser;
                     
		public RockMargin(IWpfTextView textView, TextParser textParser)
		{
			_textView = textView;
			_textParser = textParser;

			this.Width = 60;
			this.ClipToBounds = true;
			this.Background = new SolidColorBrush(Colors.LightGreen);

			_textView.TextBuffer.PostChanged += delegate(object source, EventArgs args)
			{
				this.InvalidateVisual();
			};
            
			//_textView.LayoutChanged += delegate(object source, TextViewLayoutChangedEventArgs args)
			//{
			//    this.InvalidateVisual();
			//};
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
			return (marginName == RockMargin.MarginName) ? (IWpfTextViewMargin)this : null;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
        
		protected override void OnRender(DrawingContext dc)
		{
			/*var text_lines = new List<string>();

			int maxlength = 0;
			string indent_string = new string(' ', _textView.FormattedLineSource.TabSize);
			var lines = _textView.TextBuffer.CurrentSnapshot.Lines;
			foreach (var line in lines)
			{
				string text = line.GetText();
				text = text.Replace("\t", indent_string);
				text_lines.Add(text);
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width/maxlength;
			int line_index = 0;
			foreach (var line in text_lines)
			{
				int symbol_index = 0;
				foreach (var c in line)
				{
					if (c != ' ')
					{
						double left = symbol_index * symbol_size;
						var brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
						dc.DrawRectangle(brush, null, new Rect(left, line_index * 1.6 + 1, symbol_size, 1))
					}
					symbol_index++;
				}
				line_index++;
			}*/

			var snapshot = _textView.TextSnapshot;
			var segments = _textParser.Parse(_textView);

			//	calculate max string length
			int maxlength = 0;
			foreach (var line in snapshot.Lines)
			{
				string text = line.GetText();
				maxlength = Math.Max(maxlength, text.Length);
			}

			double symbol_size = this.Width / maxlength;
			foreach (var segment in segments)
			{
				string text = snapshot.GetText(segment.span);
				ITextSnapshotLine line = snapshot.GetLineFromPosition(segment.span.Start);

				double x = (segment.span.Start - line.Start.Position) * symbol_size;
				double y = line.LineNumber * 2.0;
				double w = (segment.span.Length) * symbol_size;
				double h = 1;
				
				var brush = new SolidColorBrush(segment.color);
				dc.DrawRectangle(brush, null, new Rect(x, y, w, h));
			}
		}
	}
}