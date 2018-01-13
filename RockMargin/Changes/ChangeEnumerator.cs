using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Document;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace RockMargin
{
	class ChangeEnumerator
	{
		public struct Change
		{
			public int start_line;
			public int end_line;
			public bool saved;
		}

		private ITextView _view;
		private ITagAggregator<ChangeTag> _aggregator;
		private List<Change> _changes = new List<Change>();

		public List<Change> Changes
		{
			get { return _changes; }
		}

		public event EventHandler<EventArgs> ChangesChanged;


		public ChangeEnumerator(IViewTagAggregatorFactoryService aggregator_factory, ITextView view)
		{
			_view = view;
			_view.Closed += OnViewClosed;

			_aggregator = aggregator_factory.CreateTagAggregator<ChangeTag>(view);
			_aggregator.BatchedTagsChanged += OnTagsChanged;
		}

		void OnViewClosed(object sender, EventArgs e)
		{
			_aggregator.BatchedTagsChanged -= OnTagsChanged;
		}

		private void OnTagsChanged(object sender, BatchedTagsChangedEventArgs e)
		{
			UpdateChanges();
		}

		public void UpdateChanges()
		{
			_changes.Clear();

			var snapshot = _view.VisualSnapshot;
			var span = new SnapshotSpan(snapshot, 0, snapshot.Length);

			foreach (var tag in _aggregator.GetTags(span))
			{
				ITextBuffer buffer = tag.Span.BufferGraph.TopBuffer;
				SnapshotPoint? start_pos = tag.Span.Start.GetPoint(buffer, PositionAffinity.Successor);
				SnapshotPoint? end_pos = tag.Span.End.GetPoint(buffer, PositionAffinity.Predecessor);
				if (start_pos.HasValue && end_pos.HasValue)
				{
					var change = new Change();
					change.start_line = buffer.CurrentSnapshot.GetLineNumberFromPosition(start_pos.Value.Position);
					change.end_line = buffer.CurrentSnapshot.GetLineNumberFromPosition(end_pos.Value.Position);
					change.saved = !tag.Tag.ChangeTypes.HasFlag(ChangeTypes.ChangedSinceSaved);
					_changes.Add(change);
				}
			}

			if (ChangesChanged != null)
				ChangesChanged(this, EventArgs.Empty);
		}
	}
}
