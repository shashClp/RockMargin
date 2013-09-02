using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Classification;

namespace RockMargin
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentTypeAttribute("text")]
	[TagType(typeof(TextMarkerTag))]
	[TextViewRole(PredefinedTextViewRoles.Document)]
	class HighlightWordTaggerProvider : IViewTaggerProvider
	{
		[Import]
		internal ITextSearchService TextSearchService { get; set; }

		[Import]
		internal ITextStructureNavigatorSelectorService TextStructureNavigatorService { get; set; }

		[Import]
		internal IEditorFormatMapService FormatMapService { get; set; }


		public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
		{
			//	provide highlighting only on the top buffer
			if (view.TextBuffer != buffer)
				return null;

			var navigator = TextStructureNavigatorService.GetTextStructureNavigator(buffer);
			var format = FormatMapService.GetEditorFormatMap(view);

			return HighlightWordTagger.Instance(view as IWpfTextView, format, TextSearchService, navigator) as ITagger<T>;
		}
	}
}
