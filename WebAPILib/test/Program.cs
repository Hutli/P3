using System;
using System.Collections.Generic;
using WebAPILib;
using System.Drawing;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args) {
			string searchString = Console.ReadLine ();
			search test = new search (searchString, SearchType.ALL);
			DateTime start = DateTime.Now;
			foreach (Track t in test.Albums[0].Tracks)
				Console.WriteLine(t.Name);
			TimeSpan timeUsed = DateTime.Now - start;
			Console.WriteLine ("Cached (" + timeUsed.TotalSeconds + ")");
			foreach (Track t in test.Albums[0].Tracks)
				Console.WriteLine(t.Name);
			Console.ReadKey ();
		}
	}
}
