using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI
{
	public static class Request
	{
        /// <summary>
        /// Gets JSON from URL
        /// </summary>
        /// <param name="url">URL to JSON</param>
        /// <returns>JSON</returns>
		public async static Task<String> Get(string url)
		{
			using (var client = new HttpClient ()) {
				client.DefaultRequestHeaders.Accept.Clear ();

				HttpResponseMessage response = client.GetAsync (url).Result;
			    if (!response.IsSuccessStatusCode)
			    {
			        return null;
			    }
			    return await response.Content.ReadAsStringAsync ();
			}
		}

	}
}