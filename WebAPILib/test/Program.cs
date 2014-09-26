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
			foreach (Album a in test.Albums) {
				try{
					Console.WriteLine(a.Artists[0].Name);
				} catch {
					Console.WriteLine ("Artist not added");
				}
			}
			/*DateTime start = DateTime.Now;
			foreach (Album a in test.Albums) {
				List<Track> tmpTracks = a.Tracks;
				TimeSpan timeUsed = DateTime.Now - start;
				Console.WriteLine (string.Format("Cached {0} from {1} in {2}", a.Name, a.Artists[0].Name, timeUsed.TotalSeconds));
				start = DateTime.Now;
			}
			foreach (Album a in test.Albums) {
				Console.WriteLine (string.Format("{0} from {1} has {2} songs", a.Name, a.Artists[0].Name, a.Tracks.Count));
			}*/
			Console.ReadKey ();
		}
	}
}
