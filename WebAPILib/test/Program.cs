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
		public static void Main (string[] args)
		{
			Console.Write (get ("https://api.spotify.com/v1/search?q=test&type=track"));
			Console.ReadLine ();
		}

		public static string get(string url)
		{
			List<Track> tracks = new List<Track> ();
			WebClient client = new WebClient ();
			string content = client.DownloadString (url);
			JObject o = JObject.Parse(content);
			foreach(JObject thing in o["tracks"]["items"]){

			}
			return o["tracks"]["items"][0]["album"].ToString();
		}
			
	}
}
