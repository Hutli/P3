using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI
{
    public static class Request
    {
        /// <summary>
        ///     Gets JSON from url
        /// </summary>
        /// <param name="url">url to JSON</param>
        /// <returns>JSON</returns>
        public static async Task<String> Get(string url)
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.GetAsync(url).Result;
                if(!response.IsSuccessStatusCode)
                    return null;
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}