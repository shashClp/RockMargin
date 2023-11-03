using Microsoft.VisualStudio.Text.Tagging;

namespace RockMargin
{
	class HighlightWordTag: TextMarkerTag
	{
		public HighlightWordTag()
			: base(HighlightWordFormatDefinition.FormatName)
		{

		}
	}
}
