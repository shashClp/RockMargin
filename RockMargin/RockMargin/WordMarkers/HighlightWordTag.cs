using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;

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
