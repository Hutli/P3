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
        public VolumeModule(IUserService userService)
        {
            Get["/volume/{volPercent}/{userId}"] = parameters =>
            {
                int volPercent = parameters.volPercent;
                string userId = parameters.userId;
                if (volPercent < 0 || volPercent > 100)
                {
                    return "Volume percent must be between 0 and 100";
                }
                userService.Users.FirstOrDefault(x => x.Id == userId).Volume = (float)volPercent / 100;
                return "Successfully influenced volume";
            };
        }
    }
}
