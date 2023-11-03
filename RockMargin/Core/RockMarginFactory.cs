using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Reflection;


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
		private bool PresenceSent = false;


		public IWpfTextViewMargin CreateMargin(IWpfTextViewHost view_host, IWpfTextViewMargin container_margin)
		{
			IWpfTextView text_view = view_host.TextView;
			
			try
			{
				Utils.VSVersion = Assembly.GetCallingAssembly().GetName().Version.Major.ToString();
			}
			catch { }
			
			if (!PresenceSent)
			{
				PresenceSent = true;
				MonitoringService.SendPresense();
			}

			if (!SettingsLoaded)
				ReadSettings(OptionsService.GlobalOptions);

			RemoveVerticalScrollBar(container_margin);

			var navigator = TextStructureNavigatorService.GetTextStructureNavigator(text_view.TextBuffer);
			var format = FormatMapService.GetEditorFormatMap(text_view);
			var tagger = HighlightWordTagger.Instance(text_view, format, TextSearchService, navigator);
			var marks_enumerator = new MarksEnumerator(AggregatorFactoryService, text_view);
			var change_enumerator = new ChangeEnumerator(AggregatorFactoryService, text_view);
			var words_enumerator = new HighlightedWordsEnumerator(text_view, tagger);

			return new RockMargin(text_view, marks_enumerator, change_enumerator, words_enumerator, tagger);
		}

		private void RemoveVerticalScrollBar(IWpfTextViewMargin container_margin)
		{
			if (container_margin.GetTextViewMargin(PredefinedMarginNames.VerticalScrollBar) is IWpfTextViewMargin realScrollBarMargin)
			{
				realScrollBarMargin.VisualElement.MinWidth = 0.0;
				realScrollBarMargin.VisualElement.Width = 0.0;
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
