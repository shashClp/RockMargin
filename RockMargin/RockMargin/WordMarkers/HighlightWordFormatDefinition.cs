using Microsoft.VisualStudio.Text.Classification;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Media;
using System;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace RockMargin
{
	[Export(typeof(EditorFormatDefinition))]
	[Name(HighlightWordFormatDefinition.FormatName)]
	class HighlightWordFormatDefinition: MarkerFormatDefinition
	{
		public const string FormatName = "RockMargin/TextMarkerFormat";

		public HighlightWordFormatDefinition()
		{
			this.BackgroundColor = Colors.LightBlue;
			this.ForegroundColor = Colors.DarkBlue;
			this.ZOrder = 5;
		}

	}
}
