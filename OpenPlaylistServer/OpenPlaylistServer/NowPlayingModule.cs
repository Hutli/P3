using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class NowPlayingModule : NancyModule
    {
        public NowPlayingModule(IPlaybackService playbackService) {
            Get["/nowplaying"] = parameters =>
            {
                var track = playbackService.GetCurrentPlaying();
                if (track == null)
                    return "Nothing currently playing";
                else
                {
                    return JsonConvert.SerializeObject(track, Formatting.Indented);
                }
            };
        }
    }
}
