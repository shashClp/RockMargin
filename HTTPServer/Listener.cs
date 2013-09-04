using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPServer
{
	class Listener
	{
		private Storage _storage = new Storage();
		private HttpListener _listener = new HttpListener();
		private Dictionary<string, Action<HttpListenerContext>> _callbacks = new Dictionary<string, Action<HttpListenerContext>>();

		public Listener()
		{
			if (!HttpListener.IsSupported)
				throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class");

			RegisterPage("crash", ProcessCrashRequest);
			RegisterPage("presence", ProcessPresenceRequest);
			RegisterPage("report", ProcessReportRequest);
			RegisterPage("storage", ProcessStorageRequest);
		}

		~Listener()
		{
			_listener.Close();
		}

		public void StartListening()
		{
			_listener.Start();
			Console.WriteLine("Listening...");

			var regex = new Regex("http://.+/(.+)/", RegexOptions.Compiled);

			while (true)
			{
				HttpListenerContext context = _listener.GetContext();

				string request_url = context.Request.Url.AbsoluteUri;

				var match = regex.Match(request_url);
				if (match.Success)
				{
					string action_id = match.Groups[1].Value;

					Action<HttpListenerContext> action = null;
					if (_callbacks.TryGetValue(action_id, out action))
						action(context);
				}
			}
		}

		private void RegisterPage(string page, Action<HttpListenerContext> callback)
		{
			string address = string.Format("http://*/{0}/", page);
			_listener.Prefixes.Add(address);
			_callbacks.Add(page, callback);
		}

		private void ProcessCrashRequest(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			if (request.HasEntityBody)
			{
				var client_stream = new System.IO.StreamReader(request.InputStream, request.ContentEncoding);
				string callstack = client_stream.ReadToEnd();

				var user = new UserInfo(context.Request);
				if (user.IsValid())
				{
					_storage.AddCrashReport(user, callstack);
					Console.WriteLine(string.Format("{0} => crash", user.ToString()));
				}
			}
		}

		private void ProcessPresenceRequest(HttpListenerContext context)
		{
			var user = new UserInfo(context.Request);
			if (user.IsValid())
			{
				_storage.AddPresenceReport(user);
				Console.WriteLine(string.Format("{0} => presence", user.ToString()));
			}
		}

		private void CreateHTMLPage(HttpListenerResponse response, string data, string content_type)
		{
			// Construct a response. 
			byte[] output_buffer = System.Text.Encoding.UTF8.GetBytes(data);

			// Get a response stream and write the response to it.
			response.ContentType = content_type;
			response.ContentLength64 = output_buffer.Length;
			System.IO.Stream output = response.OutputStream;
			output.Write(output_buffer, 0, output_buffer.Length);

			// You must close the output stream.
			output.Close();
		}

		private void ProcessStorageRequest(HttpListenerContext context)
		{
			CreateHTMLPage(context.Response, _storage.GetXmlString(), "text/xml");
		}

		private void ProcessReportRequest(HttpListenerContext context)
		{
			var ips = new HashSet<string>();
			var cities = new HashSet<string>();
			uint vs_10 = 0;
			foreach (var entry in _storage.PresenceReports)
			{
				string user_hash = entry.user.IPAddress + entry.user.MachineName;

				if (!ips.Contains(user_hash))
				{
					ips.Add(user_hash);

					if (entry.user.VSVersion == "10")
						vs_10++;
				}

				if (!cities.Contains(entry.user.City))
					cities.Add(entry.user.City);
			}

			string content = @"
				<html>
					<head>
						<script type='text/javascript' src='https://www.google.com/jsapi'></script>
						<script type='text/javascript'>
						 google.load('visualization', '1', {'packages': ['geomap']});
						 google.setOnLoadCallback(drawMap);

							function drawMap() {
								var data = google.visualization.arrayToDataTable([
									['City']{0}
								]);

								var options = {};
								options['region'] = 'world';
								options['colors'] = [0xFF8747, 0xFFB581, 0xc06000]; //orange colors
								options['dataMode'] = 'markers';
								options['width'] = 800;
								options['height'] = 400;

								var container = document.getElementById('map_canvas');
								var geomap = new google.visualization.GeoMap(container);
								geomap.draw(data, options);
							};

						</script>
					</head>

					<body>
							<b>Unique users:</b> {1}<br>
							<b>Unique cities:</b> {2}<br>
							<b>VS 10:</b> {3}%<br><br>
							<div id='map_canvas'></div>
					</body>
				</html>";

			string cities_array = "";
			foreach (var city in cities)
			{
				cities_array += string.Format(",['{0}']", city);
			}
			content = content.Replace("{0}", cities_array);

			content = content.Replace("{1}", ips.Count.ToString());
			content = content.Replace("{2}", cities.Count.ToString());
			content = content.Replace("{3}", (100.0f * vs_10/ips.Count).ToString());

			CreateHTMLPage(context.Response, content, "text/html");
		}
	}
}
