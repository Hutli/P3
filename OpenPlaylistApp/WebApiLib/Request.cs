using System.Net.Http;

namespace WebAPILib
{
	public static class Request
	{
        /// <summary>
        /// Gets JSON from URL
        /// </summary>
        /// <param name="url">URL to JSON</param>
        /// <returns>JSON</returns>
		public static string Get(string url)
		{
			string str;

			using (var client = new HttpClient ()) {
				//client.BaseAddress = new URI("http://localhost:9000/");
				client.DefaultRequestHeaders.Accept.Clear ();
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				// New code:
				HttpResponseMessage response = client.GetAsync (url).Result;
			    if (!response.IsSuccessStatusCode) return null;
			    string product = response.Content.ReadAsStringAsync ().Result;
			    str = product;
			}

			return str;
		}

	}
}