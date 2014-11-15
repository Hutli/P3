using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPILib;

namespace OpenPlaylistApp.Models
{
    public class Session
    {
        public static Session session;

        private Session() {
        }

        public static Session Instance()
        {
            return session ?? (session = new Session());
        }

        public async Task<string> GetVenues()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("openplaylist.tk/venues/"))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }

        public async Task<string> SendVote(Venue venue, Track track, User user){
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, track.URI + "/" + user.Name);
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }
    }
}
