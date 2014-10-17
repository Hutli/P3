using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using SpotifyDotNet;

namespace OpenPlaylistServer
{
    public class TestNancyModule : NancyModule
    {
        public TestNancyModule()
        {
            Get["/{trackId}/{userId}/"] = parameters => { 
            Track vote = SpotifyLoggedIn.Instance.TrackFromLink(parameters.trackId);
            string userId = parameters.userId;

            MainWindow.UserVote(userId, vote);

            return "Success";
            };
        }
    }
}
