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
            foreach (Artist a in test.Albums[0].Artists)
                Console.WriteLine(a.Name);
			Console.WriteLine (test.tracks [0].Name);
            TimeSpan timeUsed = DateTime.Now - start;
			
			Console.WriteLine (timeUsed.TotalSeconds);
			Console.ReadKey ();
		}
	}
}
