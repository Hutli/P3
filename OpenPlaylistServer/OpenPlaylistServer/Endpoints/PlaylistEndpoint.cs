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
            Get["playlist"] = e => JsonConvert.SerializeObject(playlistService.Tracks, Formatting.Indented);
        }
    }
}
