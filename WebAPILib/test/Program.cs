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

namespace test {
	class MainClass {
		public static void Main (string[] args) {
			string searchString = Console.ReadLine ();
			search test = new search (searchString, SearchType.ALL);
			Console.WriteLine ("Artists: {0}", test.Artists.Count);
			foreach (Artist a in test.Artists)
				if(test.Artists.Exists (b => b != a && a.ID.Equals (b.ID)))
					Console.WriteLine ("Fuck!");
			Console.WriteLine ("Albums:  {0}", test.Albums.Count);
			foreach (Album a in test.Albums)
				if(test.Albums.Exists (b => b != a && a.ID.Equals (b.ID)))
					Console.WriteLine ("Fuck!");
			Console.WriteLine ("Tracks:  {0}", test.Tracks.Count);
			foreach (Track a in test.Tracks)
				if (test.Tracks.Exists (b => b != a && a.ID.Equals (b.ID)))
					Console.WriteLine ("Fuck!");
			Console.ReadKey ();
		}
	}
}
