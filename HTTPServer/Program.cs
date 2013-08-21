using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var listener = new Listener();
			listener.StartListening();
		}
	}
}
