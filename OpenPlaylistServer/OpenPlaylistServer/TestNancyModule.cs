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
            var session = SpotifyDotNet.Session.Instance;
            Track track = session.TrackFromLink(parameters.trackId);
            session.Play(track);
            return "Success";
            };
        }
    }
}
