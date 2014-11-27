using System;
using System.Linq;
using System.Windows;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class NowPlayingEndpoint : NancyModule
    {
        public NowPlayingEndpoint(IPlaybackService playbackService, IVoteService vs, IPlaylistService _playlistService, IUserService _userService) {
            Get["/nowplaying"] = parameters =>
            {
                var track = playbackService.GetCurrentPlaying();
                return track == null ? "Nothing currently playing" : JsonConvert.SerializeObject(track, Formatting.Indented);
            };
        }
    }
}
