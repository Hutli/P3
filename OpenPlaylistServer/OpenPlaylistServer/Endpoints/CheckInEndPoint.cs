using System.Linq;
using Nancy;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class CheckInEndPoint : NancyModule
    {
        public CheckInEndPoint(IUserService userService)
        {
            Get["/checkin/{userId}"] = parameters =>
                                       {
                                           string userId = parameters.userId;
                                           if(!userService.Users.Any(x => x.Id.Equals(userId)))
                                           {
                                               RootDispatcherFetcher.RootDispatcher.Invoke(() => userService.Add(new User(userId)));
                                               return "OK";
                                           }
                                           return "Already checked in";
                                       };

            Get["/checkout/{userId}"] = parameters =>
                                        {
                                            string userId = parameters.userId;
                                            RootDispatcherFetcher.RootDispatcher.Invoke(() =>
                                                                                        {
                                                                                            var user = userService.Users.First(x => x.Id == userId);
                                                                                            userService.Users.Remove(user);
                                                                                        });
                                            return "OK";
                                        };
        }
    }
}