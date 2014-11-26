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
            Get["/checkin/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                userService.Add(new User(userId,playbackService));
                return "OK";
            };
        }
    }
}
