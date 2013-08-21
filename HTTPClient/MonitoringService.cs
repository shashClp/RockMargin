using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HTTPClient
{
	class MonitoringService
	{
		readonly string MONITORING_SERVER = "92.249.114.70";
		readonly string VERSION = "1.0.1";
		public Timer _presenceTimer = new Timer(60 * 60 * 1000);	//	once per hour

		public MonitoringService()
		{
			_presenceTimer.Elapsed += delegate(object sender, ElapsedEventArgs e) { SendPresense(); };
			_presenceTimer.Start();
		}

		private HttpWebRequest CreateRequest(string request_type)
		{
			string uri = string.Format("http://{0}/{1}/", MONITORING_SERVER, request_type);
			var request = WebRequest.CreateHttp(uri);
			request.KeepAlive = false;

			request.CookieContainer = new CookieContainer();
			request.CookieContainer.Add(new Uri(uri), new Cookie("version", VERSION));

			return request;
		}

		private void SendPresense()
		{
			var request = CreateRequest("presence");
			request.GetResponseAsync();
		}

		public void SendCrashReport(string callstack)
		{
			var request = CreateRequest("crash");

			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(callstack);
			request.ContentType = "text/xml";
			request.Method = "POST";
			request.ContentLength = buffer.Length;

			try
			{
				var stream = request.GetRequestStream();
				stream.Write(buffer, 0, buffer.Length);

				request.GetResponseAsync();
			}
			catch { }
		}
	}
}
