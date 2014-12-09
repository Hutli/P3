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
            var ordered = tracks.OrderByDescending(t => t.TScore).Where(t => t.TotalScore != 0);
            
            Get["playlist"] = e => JsonConvert.SerializeObject(ordered, Formatting.Indented);
        }
    }
}
