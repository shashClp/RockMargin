using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace RockMargin
{
	class IPLookup
	{
		private readonly string LOOKUP_SERVER = "http://checkip.org";

		public string IPAddress { get; private set; }
		public string Country { get; private set; }
		public string City { get; private set; }

		public IPLookup()
		{
			var web_client = new WebClient();
			string data = web_client.DownloadString(LOOKUP_SERVER);

			IPAddress = FindInfo(data, @">IP Address: .*?>(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)");
			Country = FindInfo(data, @"Country: (.*?)<");
			City = FindInfo(data, @"City: (.*?)<");
		}

		private string FindInfo(string source, string pattern)
		{
			var regex = new Regex(pattern);
			var match = regex.Match(source);
			return (match.Success && match.Groups.Count > 1) ? match.Groups[1].Value : string.Empty;
		}
	}
}
