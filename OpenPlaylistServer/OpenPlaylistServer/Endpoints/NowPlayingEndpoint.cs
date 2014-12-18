using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints {
    public class NowPlayingEndpoint : NancyModule {
        public NowPlayingEndpoint(IPlaybackService playbackService) {
            Get["/nowplaying"] = parameters => {
                var track = playbackService.GetCurrentPlaying();
                return track == null
                           ? "Nothing currently playing"
                           : JsonConvert.SerializeObject(track, Formatting.Indented);
            };
        }
    }
}