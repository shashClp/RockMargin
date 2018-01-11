using System;
using System.Net;
using System.Threading;

namespace RockMargin
{
	static class MonitoringService
	{
		static readonly string MONITORING_SERVER = "92.249.114.70";

		static private HttpWebRequest CreateRequest(string request_type)
		{
			string uri = string.Format("http://{0}/{1}/", MONITORING_SERVER, request_type);
			var request = WebRequest.Create(uri) as HttpWebRequest;
			request.KeepAlive = false;

			request.CookieContainer = new CookieContainer();
			request.CookieContainer.Add(new Uri(uri), new Cookie("vsix_version", Utils.GetVsixVersion()));
			request.CookieContainer.Add(new Uri(uri), new Cookie("vs_version", Utils.VSVersion));
			request.CookieContainer.Add(new Uri(uri), new Cookie("machine_name", Environment.MachineName));

			return request;
		}

		static private void GetResponseAsync(HttpWebRequest request)
		{
			var thread = new Thread(() =>
			{
				try
				{
					var ip_lookup = new IPLookup();
					request.CookieContainer.Add(request.RequestUri, new Cookie("ip", ip_lookup.IPAddress));
					request.CookieContainer.Add(request.RequestUri, new Cookie("country", ip_lookup.Country));
					request.CookieContainer.Add(request.RequestUri, new Cookie("city", ip_lookup.City));

					request.GetResponse();
				}
				catch {}
			});
			thread.Start();
		}

		static public void SendPresense()
		{
			var request = CreateRequest("presence");
			GetResponseAsync(request);
		}

		static public void SendCrashReport(string callstack)
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
				GetResponseAsync(request);
			}
			catch { }
		}
	}
}
