using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using SpotifyDotNet;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace OpenPlaylistServer
{
    public class TestNancyModule : NancyModule
    {
        public static event Action<string,Track> UserVoted;

        public TestNancyModule(Playlist p)
        {
            Get["/{trackId}/{userId}/"] = parameters => {
                
                string userId = parameters.userId;
                try
                {
                    Track vote = SpotifyLoggedIn.Instance.TrackFromLink(parameters.trackId).Result;
                    UserVoted(userId, vote);
                }
                catch (ArgumentException ex)
                {
                    
                }
                
                return "success";
                //return Response.AsJson<object>(p._tracks);
            };

            Get["/car/{id}"] = parameters =>
            {
                int id = parameters.id;

                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(id);
            };
        }
    }
}
