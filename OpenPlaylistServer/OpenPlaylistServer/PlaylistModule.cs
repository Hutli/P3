using Nancy;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class PlaylistModule : NancyModule
    {
        public PlaylistModule(IPlaylistService playlistService)
        {
            Get["playlist"] = e => JsonConvert.SerializeObject(playlistService.Tracks, Formatting.Indented);
        }
    }
}
