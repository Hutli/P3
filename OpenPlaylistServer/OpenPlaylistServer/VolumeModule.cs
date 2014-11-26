using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace OpenPlaylistServer
{
    public class VolumeModule : NancyModule
    {
        public VolumeModule(IUserService userService, IPlaybackService playbackService)
        {
            Get["/volume/{volPercent}/{userId}"] = parameters =>
            {
                int volPercent = parameters.volPercent;
                string userId = parameters.userId;
                if (volPercent < 0 || volPercent > 100)
                {
                    return "Volume percent must be between 0 and 100";
                }
                var user = userService.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    user = new User(userId, playbackService);
                    userService.Add(user);
                }
                user.Volume = volPercent / 100F;
                return "Successfully influenced volume";
            };
        }
    }
}
