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
        public CheckInEndPoint(IUserService userService)
        {
            Get["/checkin/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                if (userService.Users.Any(x => x.Id == userId))
                {
                    RootDispatcherFetcher.RootDispatcher.Invoke(() => userService.Add(new User(userId)));
                    return "OK";
                }
                else {
                    return "Already checked in";
                }
                
            };

            Get["/checkout/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                RootDispatcherFetcher.RootDispatcher.Invoke((Action) (() =>
                {
                    var user = userService.Users.First(x => x.Id == userId);
                    userService.Users.Remove(user);
                }));
                return "OK";
            };
        }
    }
}
