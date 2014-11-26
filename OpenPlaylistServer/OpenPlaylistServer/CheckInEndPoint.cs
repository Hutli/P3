using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class CheckInEndPoint : NancyModule
    {
        public CheckInEndPoint(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService)
        {
            Get["/check/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                userService.Add(new User(userId,playbackService));
                if (playlistService.Tracks.Count == 0)
                {
                    return "Playlist empty";
                }
                return JsonConvert.SerializeObject(playlistService.Tracks, Formatting.Indented);
            };
        }
    }
}
