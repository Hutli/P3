using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using System;
using System.Linq;
using System.Windows;

namespace OpenPlaylistServer.Endpoints
{
    public class CheckInEndPoint : NancyModule
    {
        public CheckInEndPoint(IPlaylistService playlistService, IUserService userService)
        {
            Get["/checkin/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                if (userService.Users.All(x => x.Id != userId))
                {
                    Application.Current.Dispatcher.Invoke((Action)(() => userService.Add(new User(userId,playbackService))));
                }
                return "OK";
            };

            Get["/checkout/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                userService.Users.First(x => x.Id == userId).CheckedIn = false;
                return "OK";
            };
        }
    }
}
