using System.ComponentModel;
using System.Drawing;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;

namespace RockMargin
{
	public class OptionsPage : DialogPage
	{
		const string GeneralCategoryName = "General";
		const string ScrollCategoryName = "Scroll coloring";
		const string HighlightsCategoryName = "Text markers";

		public static IEditorOptionsFactoryService OptionsService;
		public static IVsSettingsManager SettingsManager;

		[Category(GeneralCategoryName)]
		[DisplayName("Width")]
		public uint Width
		{
			get { return GetOption(OptionsKeys.Width); }
			set { SetOption(OptionsKeys.Width, value); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Scroll margin")]
		public Color TrackMarginColor
		{
			get { return FromArgb(GetOption(OptionsKeys.MarginColor)); }
			set { SetOption(OptionsKeys.MarginColor, ToArgb(value)); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Scroll thumb")]
		public Color TrackThumbColor
		{
			get { return FromArgb(GetOption(OptionsKeys.ThumbColor)); }
			set { SetOption(OptionsKeys.ThumbColor, ToArgb(value)); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Text markers")]
		public Color TrackHighlightColor
		{
			get { return FromArgb(GetOption(OptionsKeys.HighlightColor)); }
			set { SetOption(OptionsKeys.HighlightColor, ToArgb(value)); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Comments")]
		public Color TrackCommentsColor
		{
			get { return FromArgb(GetOption(OptionsKeys.CommentsColor)); }
			set { SetOption(OptionsKeys.CommentsColor, ToArgb(value)); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Text")]
		public Color TrackTextColor
		{
			get { return FromArgb(GetOption(OptionsKeys.TextColor)); }
			set { SetOption(OptionsKeys.TextColor, ToArgb(value)); }
		}

		[Category(ScrollCategoryName)]
		[DisplayName("Background")]
		public Color TrackBackgroundColor
		{
			get { return FromArgb(GetOption(OptionsKeys.BackgroundColor)); }
			set { SetOption(OptionsKeys.BackgroundColor, ToArgb(value)); }
		}

		[Category(HighlightsCategoryName)]
		[DisplayName("Enabled")]
		public bool HighlightsEnabled
		{
			get { return GetOption(OptionsKeys.HighlightsEnabled); }
			set { SetOption(OptionsKeys.HighlightsEnabled, value); }
		}

		[Category(HighlightsCategoryName)]
		[DisplayName("Marker background color")]
		public Color HighlightBackgroundColor
		{
			get { return FromArgb(GetOption(OptionsKeys.TextMarkerBackgroundColor)); }
			set { SetOption(OptionsKeys.TextMarkerBackgroundColor, ToArgb(value)); }
		}

		[Category(HighlightsCategoryName)]
		[DisplayName("Marker foreground color")]
		public Color HighlightForegroundColor
		{
			get { return FromArgb(GetOption(OptionsKeys.TextMarkerForegroundColor)); }
			set { SetOption(OptionsKeys.TextMarkerForegroundColor, ToArgb(value)); }
		}


		private Color FromArgb(uint argb)
		{
			return Color.FromArgb((int)argb);
		}

		private uint ToArgb(Color color)
		{
			return (uint)(color.A << 24 | color.R << 16 | color.G << 8 | color.B);
		}

		private void SetOption<T>(EditorOptionKey<T> key, T value)
		{
			OptionsService.GlobalOptions.SetOptionValue(key, value);
		}

		private T GetOption<T>(EditorOptionKey<T> key)
		{
			return OptionsService.GlobalOptions.GetOptionValue(key);
		}

		public override void LoadSettingsFromStorage()
		{
			var s = new SettingsStore(SettingsManager, OptionsService.GlobalOptions);
			s.Load();
		}

		public override void SaveSettingsToStorage()
		{
			var s = new SettingsStore(SettingsManager, OptionsService.GlobalOptions);
			s.Save();
		}
	}
}
