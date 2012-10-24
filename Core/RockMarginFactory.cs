using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows.Media;


namespace RockMargin
{
	[Export(typeof(IWpfTextViewMarginProvider))]
	[Name(RockMargin.MarginName)]
	[MarginContainer(PredefinedMarginNames.VerticalScrollBarContainer)]
	[Order(After = PredefinedMarginNames.VerticalScrollBar)]
	[ContentType("text")]
	[TextViewRole(PredefinedTextViewRoles.Document)]
	internal sealed class RockMarginFactory: IWpfTextViewMarginProvider
	{
		[Import]
		public IViewTagAggregatorFactoryService AggregatorFactoryService { get; set; }

		[Import]
		internal ITextSearchService TextSearchService { get; set; }

		[Import]
		internal ITextStructureNavigatorSelectorService TextStructureNavigatorService { get; set; }

		[Import]
		internal IEditorOptionsFactoryService OptionsService { get; set; }

		[Import]
		internal IEditorFormatMapService FormatMapService { get; set; }

		[Import(typeof(SVsServiceProvider))]
		internal IServiceProvider ServiceProvider { get; set; }

		private bool SettingsLoaded = false;


		public IWpfTextViewMargin CreateMargin(IWpfTextViewHost view_host, IWpfTextViewMargin container_margin)
		{
			IWpfTextView text_view = view_host.TextView;

			if (!SettingsLoaded)
				ReadSettings(OptionsService.GlobalOptions);

			RemoveVerticalScrollBar(container_margin);

			var navigator = TextStructureNavigatorService.GetTextStructureNavigator(text_view.TextBuffer);
			var format = FormatMapService.GetEditorFormatMap(text_view);
			var tagger = HighlightWordTagger.Instance(text_view, format, TextSearchService, navigator);
			var marks_enumerator = new MarksEnumerator(AggregatorFactoryService, text_view);
			var words_enumerator = new HighlightedWordsEnumerator(text_view, tagger);

			return new RockMargin(text_view, marks_enumerator, words_enumerator, tagger);
		}

		private void RemoveVerticalScrollBar(IWpfTextViewMargin container_margin)
		{
			var realScrollBarMargin = container_margin.GetTextViewMargin(PredefinedMarginNames.VerticalScrollBar) as IWpfTextViewMargin;
			if (realScrollBarMargin != null)
			{
				var panel = container_margin as System.Windows.Controls.Panel;
				if (panel != null)
					panel.Children.Remove(realScrollBarMargin as System.Windows.UIElement);
			}
		}

		private void ReadSettings(IEditorOptions options)
		{
			var settings_manager = ServiceProvider.GetService(typeof(SVsSettingsManager)) as IVsSettingsManager;
			var store = new SettingsStore(settings_manager, options);
			store.Load();
			SettingsLoaded = true;
		}
	}
}
