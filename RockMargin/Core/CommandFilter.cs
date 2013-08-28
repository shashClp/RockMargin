using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RockMargin
{
	class CommandFilter : IOleCommandTarget
	{
		private IOleCommandTarget _nextTarget;
		private HighlightWordTagger _tagger;


		public CommandFilter(IVsTextView view_adapter, HighlightWordTagger tagger)
		{
			_tagger = tagger;
			view_adapter.AddCommandFilter(this, out _nextTarget);
		}

		int IOleCommandTarget.QueryStatus(ref Guid cmd_group, uint cmds_count, OLECMD[] cmds, IntPtr cmd_text)
		{
			return _nextTarget.QueryStatus(ref cmd_group, cmds_count, cmds, cmd_text);
		}

		int IOleCommandTarget.Exec(ref Guid cmd_group, uint cmd_id, uint cmd_exec_opt, IntPtr in_args, IntPtr out_args)
		{
			if (cmd_group == VSConstants.VSStd2K && cmd_id == (uint)VSConstants.VSStd2KCmdID.CANCEL)
			{
				_tagger.Clear();
			}

			return _nextTarget.Exec(ref cmd_group, cmd_id, cmd_exec_opt, in_args, out_args);
		}
	}
}
