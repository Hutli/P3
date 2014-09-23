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
			DateTime start = DateTime.Now;
			search test = new search (searchString, SearchType.ALL);
			TimeSpan timeUsed = DateTime.Now - start;
			foreach (SpotifyObject o in test.results) {
				Console.WriteLine(string.Format("{0} of type {1}", o.Name, o.GetType()));
			}
			Console.WriteLine (timeUsed.TotalSeconds);
			Console.ReadKey ();
		}
	}
}
