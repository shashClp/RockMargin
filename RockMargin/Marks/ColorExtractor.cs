using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RockMargin
{
	public static class ColorExtractor
	{
		private static Microsoft.VisualStudio.OLE.Interop.IServiceProvider _globalServiceProvider;
		private static IVsFontAndColorUtilities _fontAndColorUtilities;

		internal static Microsoft.VisualStudio.OLE.Interop.IServiceProvider GlobalServiceProvider
		{
			get
			{
				if (_globalServiceProvider == null)
					_globalServiceProvider = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)(Package.GetGlobalService(typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)));

				return _globalServiceProvider;
			}
			set
			{
				_globalServiceProvider = value;
			}
		}

		internal static InterfaceType GetService<InterfaceType, ServiceType>(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider)
		{
			return (InterfaceType)GetService(serviceProvider, typeof(ServiceType).GUID, false);
		}

		internal static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider, Guid guidService, bool unique)
		{
			Guid guidInterface = VSConstants.IID_IUnknown;
			IntPtr ptrObject = IntPtr.Zero;
			object service = null;

			int hr = serviceProvider.QueryService(ref guidService, ref guidInterface, out ptrObject);
			if (hr >= 0 && ptrObject != IntPtr.Zero)
			{
				try
				{
					if (unique)
					{
						service = Marshal.GetUniqueObjectForIUnknown(ptrObject);
					}
					else
					{
						service = Marshal.GetObjectForIUnknown(ptrObject);
					}
				}
				finally
				{
					Marshal.Release(ptrObject);
				}
			}

			return service;
		}

		internal static IVsFontAndColorUtilities FontAndColorUtilities
		{
			get
			{
				if (_fontAndColorUtilities == null)
					_fontAndColorUtilities = GetService<IVsFontAndColorUtilities, SVsFontAndColorStorage>(GlobalServiceProvider);

				return _fontAndColorUtilities;
			}
		}

		public static Brush GetBrushFromIndex(COLORINDEX color_index)
		{
			UInt32 w32color;
			Marshal.ThrowExceptionForHR(FontAndColorUtilities.GetRGBOfIndex(color_index, out w32color));

			System.Drawing.Color gdiColor = System.Drawing.ColorTranslator.FromWin32((int)w32color);

			Color color = Color.FromArgb(gdiColor.A, gdiColor.R, gdiColor.G, gdiColor.B);
			return new SolidColorBrush(color);
		}
	}
}
