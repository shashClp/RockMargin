using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RockMargin
{
	static class Utils
	{
		public static Color CreateColor(uint argb)
		{
			byte a, r, g, b;
			UnpackArgb(argb, out a, out r, out g, out b);

			return Color.FromArgb(a, r, g, b);
		}

		public static Brush CreateBrush(uint argb)
		{
			Color color = CreateColor(argb);
			return new SolidColorBrush(color);
		}

		public static uint PackArgb(byte a, byte r, byte g, byte b)
		{
			return ((uint)a << 24) | ((uint)r << 16) | ((uint)g << 8) | ((uint)b << 0);
		}

		public static void UnpackArgb(uint argb, out byte a, out byte r, out byte g, out byte b)
		{
			a = (byte)(argb >> 24);
			r = (byte)(argb >> 16);
			g = (byte)(argb >> 8);
			b = (byte)(argb);
		}
	}
}
