using System.Linq;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Endpoints
{
    public class PlaylistEndpoint : NancyModule
    {
        public PlaylistEndpoint(IPlaylistService playlistService)
        {
            var tracks = playlistService.Tracks;
            
            Get["playlist"] = e => JsonConvert.SerializeObject(tracks.OrderByDescending(t => t.TScore), Formatting.Indented);
        }
    }
}
