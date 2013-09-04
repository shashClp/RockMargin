using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace HTTPServer
{
	class Storage
	{
		private readonly string STORAGE_FILE = "storage.xml";
		private readonly TimeSpan PERSISTANCE_INTERVAL = new TimeSpan(0, 0, 1);

		private DateTime _lastSave = DateTime.MinValue;

		public struct Entry
		{
			public UserInfo user;
			public DateTime date;

			public Entry(UserInfo user)
			{
				this.user = user;
				this.date = DateTime.Now;
			}

			public void Serialize(XmlElement node)
			{
				user.SerializeToXml(node);
				node.SetAttribute("date", date.ToString("dd.MM.yyyy H:mm:ss"));
			}

			public static Entry Deserialize(XmlElement node)
			{
				string s = node.GetAttribute("date");
				return new Entry {
					user = new UserInfo(node),
					date = DateTime.ParseExact(node.GetAttribute("date"), "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture)
				};
			}
		}

		public List<Entry> PresenceReports { get; private set; }
		public Dictionary<Entry, string> CrashReports { get; private set; }

		public Storage()
		{
			PresenceReports = new List<Entry>();
			CrashReports = new Dictionary<Entry, string>();

			Load();
		}
				
		public void AddPresenceReport(UserInfo user)
		{
			bool exists = PresenceReports.Exists((Entry e) =>
				e.user.IPAddress == user.IPAddress &&
				e.user.MachineName == user.MachineName &&
				e.date.Date == DateTime.Today);

			if (!exists)
			{
				PresenceReports.Add(new Entry(user));
				PersistanceCheckpoint();
			}
		}

		public void AddCrashReport(UserInfo user, string callstack)
		{
			var entry = new Entry(user);
			if (!CrashReports.ContainsKey(entry))
			{
				CrashReports.Add(entry, callstack);
				PersistanceCheckpoint();
			}
		}

		private void PersistanceCheckpoint()
		{
			if (DateTime.Now > _lastSave.Add(PERSISTANCE_INTERVAL))
			{
				Save();
			}
		}

		public string GetXmlString()
		{
			XmlDocument doc = GenerateXml();
			return doc.InnerXml;
		}

		private XmlDocument GenerateXml()
		{
			XmlDocument doc = new XmlDocument();
			var root = doc.CreateElement("storage");

			foreach (var report in PresenceReports)
			{
				var node = doc.CreateElement("presence");
				report.Serialize(node);
				root.AppendChild(node);
			}

			foreach (var report in CrashReports)
			{
				var node = doc.CreateElement("crash");
				report.Key.Serialize(node);
				node.AppendChild(doc.CreateTextNode(report.Value));
				root.AppendChild(node);
			}

			doc.AppendChild(root);
			return doc;
		}

		private void Save()
		{
			XmlDocument doc = GenerateXml();
			doc.Save(STORAGE_FILE);
			_lastSave = DateTime.Now;
		}

		private void Load()
		{
			PresenceReports.Clear();
			CrashReports.Clear();

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(STORAGE_FILE);
				foreach (XmlElement node in doc.DocumentElement.ChildNodes)
				{
					Entry entry = Entry.Deserialize(node);
					
					if (!entry.user.IsValid())
						continue;

					switch (node.Name)
					{
						case "presence":
							PresenceReports.Add(entry);
							break;

						case "crash":
							CrashReports.Add(entry, node.InnerText);
							break;
					}
				}
			}
			catch (FileNotFoundException) { }
		}
	}
}
