using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Editor;

namespace RockMargin
{
	public static class OptionsKeys
	{
		public static readonly EditorOptionKey<uint> Width = new EditorOptionKey<uint>("RockMargin/Width");

		public static readonly EditorOptionKey<bool> AltHighlights = new EditorOptionKey<bool>("RockMargin/AltHighlights");
		public static readonly EditorOptionKey<bool> HighlightsEnabled = new EditorOptionKey<bool>("RockMargin/HighlightsEnabled");
		public static readonly EditorOptionKey<uint> MarginColor = new EditorOptionKey<uint>("RockMargin/MarginColor");
		public static readonly EditorOptionKey<uint> ScrollColor = new EditorOptionKey<uint>("RockMargin/ScrollColor");
		public static readonly EditorOptionKey<uint> ThumbColor = new EditorOptionKey<uint>("RockMargin/ThumbColor");
		public static readonly EditorOptionKey<uint> TextColor = new EditorOptionKey<uint>("RockMargin/TextColor");
		public static readonly EditorOptionKey<uint> CommentsColor = new EditorOptionKey<uint>("RockMargin/CommentsColor");
		public static readonly EditorOptionKey<uint> HighlightColor = new EditorOptionKey<uint>("RockMargin/HighlightColor");
		public static readonly EditorOptionKey<uint> BackgroundColor = new EditorOptionKey<uint>("RockMargin/BackgroundColor");

		public static readonly EditorOptionKey<uint> TextMarkerBackgroundColor = new EditorOptionKey<uint>("RockMargin/TextMarkerBackgroundColor");
		public static readonly EditorOptionKey<uint> TextMarkerForegroundColor = new EditorOptionKey<uint>("RockMargin/TextMarkerForegroundColor");
	}
}
