using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HTTPServer
{
	class UserInfo
	{
		public string IPAddress { get; private set; }
		public string VsixVersion { get; private set; }
		public string VSVersion { get; private set; }
		public string MachineName { get; private set; }
		public string Country { get; private set; }
		public string City { get; private set; }

		public UserInfo(HttpListenerRequest request)
		{
			IPAddress = ExtractInfo(request, "ip");
			VsixVersion = ExtractInfo(request, "vsix_version");
			VSVersion = ExtractInfo(request, "vs_version");
			MachineName = ExtractInfo(request, "machine_name");
			Country = ExtractInfo(request, "country");
			City = ExtractInfo(request, "city");
		}

		public UserInfo(XmlElement node)
		{
			IPAddress = node.GetAttribute("ip");
			VsixVersion = node.GetAttribute("vsix_version");
			VSVersion = node.GetAttribute("vs_version");
			MachineName = node.GetAttribute("machine_name");
			Country = node.GetAttribute("country");
			City = node.GetAttribute("city");
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(IPAddress) &&
				!string.IsNullOrEmpty(VsixVersion) &&
				!string.IsNullOrEmpty(VSVersion) &&
				!string.IsNullOrEmpty(MachineName);
		}

		public void SerializeToXml(XmlElement node)
		{
			node.SetAttribute("ip", IPAddress);
			node.SetAttribute("vsix_version", VsixVersion);
			node.SetAttribute("vs_version", VSVersion);
			node.SetAttribute("machine_name", MachineName);
			node.SetAttribute("country", Country);
			node.SetAttribute("city", City);
		}

		private string ExtractInfo(HttpListenerRequest request, string info)
		{
			var cookie = request.Cookies[info];
			return cookie == null ? string.Empty : cookie.Value;
		}

		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}, VS{3}, {4}/{5}]", IPAddress, MachineName, VsixVersion, VSVersion, Country, City);
		}
	}
}
