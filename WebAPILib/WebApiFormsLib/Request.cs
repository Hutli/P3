using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;

namespace WebAPILib
{
	public static class Request
	{

		public static string get(string url)
		{
			string str = null;

			using (var client = new HttpClient ()) {
				//client.BaseAddress = new Uri("http://localhost:9000/");
				client.DefaultRequestHeaders.Accept.Clear ();
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				// New code:
				HttpResponseMessage response = client.GetAsync (url).Result;
				if (response.IsSuccessStatusCode) {
					string product = response.Content.ReadAsStringAsync ().Result;
					str = product;
				}
			}

			return str;
		}

	}
}

