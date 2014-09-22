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
	public class search
	{
	
	public string get(string url)
		{
			List<Track> tracks = new List<Track> ();
			WebClient client = new WebClient ();
			string content = client.DownloadString (url);
			return content;
		}
	}
}

