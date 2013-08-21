using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPClient
{
	class Program
	{
		static MonitoringService monitoring = new MonitoringService();

		static void Crash()
		{
			WebRequest a = null;
			a.Abort();
		}

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			Thread.Sleep(2000);
			Crash();
		}

		static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				monitoring.SendCrashReport(ex.StackTrace);
			}
		}
	}
}
