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
                Track vote = SpotifyLoggedIn.Instance.TrackFromLink(parameters.trackId).Result;
                string userId = parameters.userId;
                //MainWindow.UserVote(userId, vote);
            
                UserVoted(userId, vote);

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
