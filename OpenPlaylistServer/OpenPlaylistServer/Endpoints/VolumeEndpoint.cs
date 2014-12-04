using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using Nancy;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class VolumeEndpoint : NancyModule
    {
        public VolumeEndpoint(IUserService userService, IPlaybackService playbackService)
        {
            Get["/volume/{volPercent}/{userId}"] = parameters =>
            {
                int volPercent = parameters.volPercent;
                string userId = parameters.userId;

                if (volPercent >= 0 && volPercent <= 100)
                {
                    var user = userService.Users.FirstOrDefault(x => x.Id == userId);
                    if (user != null)
                    {
                        user.Volume = volPercent / 100F;
                        playbackService.RefreshCurrentVolume();
                    }
                }

                return "" + Convert.ToInt32(playbackService.GetCurrentVolume()*100);
            };
        }
    }
}
