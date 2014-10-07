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
			foreach (Album a in test.Albums) {
				timeBefore = DateTime.Now;
				List<Artist> tmpArtists = a.Artists;
				Console.WriteLine (string.Format ("{0, -30} Cached: {1}", a.Name, (DateTime.Now - timeBefore).TotalSeconds < 0.01));
				foreach (Artist b in a.Artists) {
					if (!testArtists.Exists (c => b.ID.Equals (c.ID)))
						testArtists.Add (b);
				}
			}

			List<Artist> nullArtistAlbums = new List<Artist> ();
			foreach (Artist a in test.Artists) {
				if (a.Albums.Count == 0) {
					nullArtistAlbums.Add (a);
				}
			}

			testArtists.AddRange (nullArtistAlbums);
			List<Artist> originalArtists = new List<Artist> (test.Artists);


			testArtists.Sort((a, b) => a.Name.CompareTo(b.Name));
			originalArtists.Sort ((a, b) => a.Name.CompareTo(b.Name));

			Console.WriteLine ("Album-Artists: {0}\nNull-Album-Artists: {1}\nTotal Artists: {2}\nTotal Albums: {3}\nTotal Tracks: {4}", 
				testArtists.Count, nullArtistAlbums.Count, originalArtists.Count, test.Albums.Count, test.Tracks.Count);

			foreach (Artist a in test.Artists) {
				if (test.Artists.Exists (b => a != b && a.ID.Equals (b.ID)))
					Console.WriteLine ("FUCK!");
			}

			Console.ReadLine ();
		}
	}
}
