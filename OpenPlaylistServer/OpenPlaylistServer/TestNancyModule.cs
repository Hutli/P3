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
            Get["/{trackId}"] = parameters => { 
            var session = SpotifyDotNet.Spotify.Instance;
            Track track = SpotifyLoggedIn.Instance.TrackFromLink(parameters.trackId);
            var spotifyLoggedIn = SpotifyLoggedIn.Instance;

            MainWindow.NancyRequest(track);
            return "Success";
            };
        }
    }
}
