using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using System;
using System.Windows;

namespace OpenPlaylistServer.Endpoints
{
    public class CheckInEndPoint : NancyModule
    {
        public CheckInEndPoint(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService)
        {
            Get["/checkin/{userId}"] = parameters =>
            {
                string userId = parameters.userId;
                Application.Current.Dispatcher.BeginInvoke((Action)(() => userService.Add(new User(userId,playbackService))));
                return "OK";
            };
        }
    }
}
