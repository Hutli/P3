using System.Linq;
using System.Windows;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation
{
    public class VoteService : IVoteService
    {
        private IPlaylistService _playlistService;
        private IUserService _userService;

        public VoteService(IPlaylistService playlistService, IUserService userService) {
            _playlistService = playlistService;
            _userService = userService;
        }

        public bool Vote(string userId, string trackUri)
        {
            User user;
            Track oldVote = null;
            
            
            // is playlistTrack already voted on?
            Track playlistTrack = _playlistService.FindTrack(trackUri);
            if (playlistTrack == null)
            {
                // playlistTrack is not already voted on, so creating new instance and adding to list
                var track = WebAPIMethods.GetTrack(trackUri).Result;
                if (track == null)
                {
                    return false;
                }

                playlistTrack = track;

                RootDispatcherFetcher.RootDispatcher.Invoke(() => _playlistService.Add(playlistTrack));
                //_playlistService.Add(playlistTrack);
            }


            // Is user known?
            if (_userService.Users.Any(x => x.Id == userId))
            {
                // User is known
                user = _userService.Users.FirstOrDefault(x => x.Id == userId);

                // If user has already voted
                if (user != null && user.Vote != null)
                {
                    // remove 1 vote on old track
                    oldVote = user.Vote;
                    
                }
            }
            else
            {
                // user is not known. Adding user to list of known users
                user = new User(userId);
                //  set user's vote to new track

                RootDispatcherFetcher.RootDispatcher.Invoke(() => _userService.Add(user));
            }

            if (user != null) user.Vote = playlistTrack;

            RootDispatcherFetcher.RootDispatcher.Invoke(() =>
            {
                playlistTrack.TScore = _playlistService.CalcTScore(playlistTrack);
            });
            
            if (oldVote != null)
            {
                RootDispatcherFetcher.RootDispatcher.Invoke(() =>
                {
                    oldVote.TScore = _playlistService.CalcTScore(oldVote);
                    if (oldVote.TotalScore == 0)
                    {
                        _playlistService.Remove(oldVote);
                    }
                });
                
            }

            return true;
        }
    }
}
