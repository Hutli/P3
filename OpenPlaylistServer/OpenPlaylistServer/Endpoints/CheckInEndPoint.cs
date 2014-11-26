using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class CheckInEndPoint : NancyModule
    {
        public CheckInEndPoint(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService)
        {
            Get["/check/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                userService.Add(new User(userId,playbackService));
                return playlistService.Tracks.Count == 0 ? "Playlist empty" : JsonConvert.SerializeObject(playlistService.Tracks, Formatting.Indented);
            };
        }
    }
}
