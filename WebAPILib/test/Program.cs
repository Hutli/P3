using System;
using System.Collections.Generic;
using WebAPILib;
using System.Drawing;
using System.Web;
using System.IO;
using System.Text;
using System.Net;

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
			try {
				//string url = "https://api.spotify.com/v1/search?q=test&type=track";
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.Method = "GET";

				request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36";
				request.Accept = "application/json";
				request.AllowAutoRedirect = true;

				string responseText;
				using (var response = request.GetResponse()) {
					using (var reader = new StreamReader(response.GetResponseStream())) {
						responseText = reader.ReadToEnd ();
						return responseText;
					}
				}
			}
			catch (WebException exception) {
				//TODO handle connection error
			}
			return null; //fatal error
		}
	}
}
