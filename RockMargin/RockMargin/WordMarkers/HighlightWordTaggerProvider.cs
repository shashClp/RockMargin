using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RockMargin
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentTypeAttribute("text")]
	[TagType(typeof(TextMarkerTag))]
	[TextViewRole(PredefinedTextViewRoles.Document)]
	class HighlightWordTaggerProvider: IViewTaggerProvider
	{
		[Import]
		internal ITextSearchService TextSearchService { get; set; }

		[Import]
		internal ITextStructureNavigatorSelectorService TextStructureNavigatorService { get; set; }

		[Import]
		internal IEditorFormatMapService FormatMapService { get; set; }

		[Import]
		IVsEditorAdaptersFactoryService AdaptersService { get; set; }


		public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
		{
			//	provide highlighting only on the top buffer
			if (view.TextBuffer != buffer)
				return null;

			var navigator = TextStructureNavigatorService.GetTextStructureNavigator(buffer);
			var format = FormatMapService.GetEditorFormatMap(view);
			var tagger = HighlightWordTagger.Instance(view as IWpfTextView, format, TextSearchService, navigator);

			IVsTextView view_adapter = AdaptersService.GetViewAdapter(view);
			new CommandFilter(view_adapter, tagger);

			return tagger as ITagger<T>;
		}
	}
}
