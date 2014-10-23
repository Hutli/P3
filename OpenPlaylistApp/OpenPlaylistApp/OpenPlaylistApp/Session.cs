using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using WebAPILib;

namespace OpenPlaylistApp
{
    public class Session
    {
        public static Session session;

        private Session() {
        }

        public static Session Instance()
        {
            if (session == null)
            {
                session = new Session();
            }
            return session;
        }
        

        public async void SendVote(Venue venue, Track track, User user){
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(venue.IP + "/" + track.URI + "/" + user.Name))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                if (result != null)
                {

                }
            }
        }
    }
}
