using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using System.Windows.Media;


namespace RockMargin
{
	class TextMark
	{
		public enum MarkType
		{
			Unknown,
			Bookmark,
			Breakpoint,
			Tracepoint
		}

		public int line;
		public MarkType type;

		private static MarkType GetMarkType(IVsVisibleTextMarkerTag tag)
		{
			tag.MarkerType.GetDisplayName(out string name);

			if (name.StartsWith("Breakpoint"))
				return MarkType.Breakpoint;

			if (name.StartsWith("Tracepoint"))
				return MarkType.Tracepoint;

			if (name.StartsWith("Bookmark"))
				return MarkType.Bookmark;

			return MarkType.Unknown;
		}

		public static TextMark Create(IMappingTagSpan<IVsVisibleTextMarkerTag> tag)
		{
			MarkType mark_type = GetMarkType(tag.Tag);
			if (mark_type == MarkType.Unknown)
				return null;

			ITextBuffer buffer = tag.Span.BufferGraph.TopBuffer;
			SnapshotPoint? pos = tag.Span.Start.GetPoint(buffer, PositionAffinity.Successor);
			if (!pos.HasValue)
				return null;

			return new TextMark()
			{
				line = buffer.CurrentSnapshot.GetLineNumberFromPosition(pos.Value.Position),
				type = mark_type
			};
		}
	}

	class MarksEnumerator
	{
		private ITagAggregator<IVsVisibleTextMarkerTag> _aggregator = null;
		private ITextView _view;

		public List<TextMark> Marks { get; } = new List<TextMark>();

		public event EventHandler<EventArgs> MarksChanged;


		public MarksEnumerator(IViewTagAggregatorFactoryService aggregator_factory, ITextView view)
		{
			_view = view;
			_view.Closed += OnViewClosed;

			_aggregator = aggregator_factory.CreateTagAggregator<IVsVisibleTextMarkerTag>(view);
			_aggregator.BatchedTagsChanged += OnTagsChanged;
		}

		void OnViewClosed(object sender, EventArgs e)
		{
			_aggregator.BatchedTagsChanged -= OnTagsChanged;
		}

		private void OnTagsChanged(object source, BatchedTagsChangedEventArgs e)
		{
			UpdateMarks();
		}

		public void UpdateMarks()
		{
			Marks.Clear();

			var snapshot = _view.VisualSnapshot;
			var span = new SnapshotSpan(snapshot, 0, snapshot.Length);
			foreach (var tag in _aggregator.GetTags(span))
			{
				var mark = TextMark.Create(tag);
				if (mark != null)
					Marks.Add(mark);
			}

			MarksChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
