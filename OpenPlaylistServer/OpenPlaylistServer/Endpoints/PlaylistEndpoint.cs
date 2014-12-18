using System.Linq;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints {
    public class PlaylistEndpoint : NancyModule {
        public PlaylistEndpoint(IPlaylistService playlistService) {
            var tracks = playlistService.Tracks;
            var ordered =
                tracks.OrderByDescending(t => t.TotalScore).Where(t => t.TotalScore != 0).Select((t, index) => {
                    t.Rank = index + 1;
                    return t;
                });

            Get["playlist"] = e => JsonConvert.SerializeObject(ordered, Formatting.Indented);
        }
    }
}