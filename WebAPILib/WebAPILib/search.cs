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

namespace WebAPILib
{
	public enum SearchType {ALL, ARTIST, ALBUM, TRACK};

	public class search
	{

		List<Artist> artists = new List<Artist> ();
		List<SpotifyObject> results = new List<SpotifyObject>();

		public search (string searchString) : this (searchString, SearchType.ALL) { }

		public search(string searchString, SearchType type)
		{

		}


		public static string get(string url)
		{
			List<Track> tracks = new List<Track> ();
			WebClient client = new WebClient ();
			string content = client.DownloadString (url);
			return content;
		}
	}
}

