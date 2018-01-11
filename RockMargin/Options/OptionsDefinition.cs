using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;

namespace RockMargin
{
	[Export(typeof(EditorOptionDefinition))]
	public sealed class WidthOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 75; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.Width; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class AltHighlightsOption : ViewOptionDefinition<bool>
	{
		public override bool Default { get { return false; } }
		public override EditorOptionKey<bool> Key { get { return OptionsKeys.AltHighlights; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class HighlightsEnabledOption : ViewOptionDefinition<bool>
	{
		public override bool Default { get { return true; } }
		public override EditorOptionKey<bool> Key { get { return OptionsKeys.HighlightsEnabled; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class MarginColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xfff0f0f0; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.MarginColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class ScrollColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0x00000000; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.ScrollColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class ThumbColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0x400000ff; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.ThumbColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class TextColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xff505050; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.TextColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class CommentsColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xff00a000; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.CommentsColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class HighlightColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xffff0000; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.HighlightColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class BackgroundColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xffffffff; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.BackgroundColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class TextMarkerBackgroundColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xffadd8e6; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.TextMarkerBackgroundColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class TextMarkerForegroundColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xff00008b; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.TextMarkerForegroundColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class SavedChangeColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xff6ce26c; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.SavedChangeColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class UnsavedChangeColorOption : ViewOptionDefinition<uint>
	{
		public override uint Default { get { return 0xffffee62; } }
		public override EditorOptionKey<uint> Key { get { return OptionsKeys.UnsavedChangeColor; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class ChangeMarginEnabledOption : ViewOptionDefinition<bool>
	{
		public override bool Default { get { return true; } }
		public override EditorOptionKey<bool> Key { get { return OptionsKeys.ChangeMarginEnabled; } }
	}

	[Export(typeof(EditorOptionDefinition))]
	public sealed class EnhancedTextRenderingOption : ViewOptionDefinition<bool>
	{
		public override bool Default { get { return true; } }
		public override EditorOptionKey<bool> Key { get { return OptionsKeys.EnhancedTextRendering; } }
	}
}
