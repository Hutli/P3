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
			DateTime timeBefore = DateTime.Now;
			search test = new search (searchString, SearchType.ALL);
			Console.WriteLine ((DateTime.Now - timeBefore).TotalSeconds);
			List<Artist> testArtists = new List<Artist> ();
			int cachedArtists = 0;
			int nonCachedArtists = 0;
			foreach (Album a in test.Albums) {
				timeBefore = DateTime.Now;
				Console.WriteLine (a.Images.Count);
				foreach (Artist b in a.Artists) {
					if (!testArtists.Contains(b)) {
						//Console.WriteLine (string.Format ("{0} | {1}", b.Name, (DateTime.Now - timeBefore).TotalSeconds));
						testArtists.Add (b);
						if ((DateTime.Now - timeBefore).TotalSeconds > 0.01) {
							nonCachedArtists++;
						} else {
							cachedArtists++;
						}
					}
				}
			}
			int nullArtistAlbums = 0;
			foreach (Artist a in test.Artists) {
				if (a.Albums.Count == 0)
					testArtists.Add (a);
					nullArtistAlbums++;
			}
			foreach (Artist a in test.Artists) {
				if (test.Artists.Exists (b => a != b && a.ID.Equals (b.ID)))
					Console.WriteLine ("FUCK!");
			}
			testArtists.Sort ((a, b) => a.Name.CompareTo(b.Name));
			List<Artist> originalArtists = test.Artists;
			originalArtists.Sort ((a, b) => a.Name.CompareTo(b.Name));
			for (int i = 0; i < testArtists.Count; i++) {
				Console.WriteLine (string.Format ("{0, -30} {1}", originalArtists [i].Name, testArtists [i].Name));
			}
			Console.WriteLine (test.Artists [4].Name + " | " + test.Albums.Exists(a => a.ID.Equals(test.Artists[4].Albums[0].ID)));
			//Console.WriteLine ("Cached: {0}\nNon-Cached: {1}\nNullAlbums: {2}\nTotal: {3}", cachedArtists, nonCachedArtists, nullArtistAlbums, test.Artists.Count);
			Console.ReadLine ();
		}
	}
}
