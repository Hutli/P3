using System;
using System.Linq;
using System.Windows;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Services.Implementation
{
    public class VoteService : IVoteService
    {
        private IPlaybackService _playbackService;
        private IPlaylistService _playlistService;
        private IUserService _userService;

        public VoteService(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService) {
            _playbackService = playbackService;
            _playlistService = playlistService;
            _userService = userService;
        }

        public void Vote(string userId, string trackUri)
        {
            User user;
            PlaylistTrack oldVote = null;
            
            
            // is playlistTrack already voted on?
            PlaylistTrack playlistTrack = _playlistService.FindTrack(trackUri);
            if (playlistTrack == null)
            {
                // playlistTrack is not already voted on, so creating new instance and adding to list
                playlistTrack = new PlaylistTrack(trackUri);

                Application.Current.Dispatcher.BeginInvoke((Action)(() => _playlistService.Add(playlistTrack)));
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
                    
                    //oldVote.TScore -= 1;
                }
            }
            else
            {
                // user is not known. Adding user to list of known users
                user = new User(userId, _playbackService);
                
                Application.Current.Dispatcher.BeginInvoke((Action)(() => _userService.Add(user)));
            }

            //  set user's vote to new track
            if (user == null) return;
            user.Vote = playlistTrack;
            playlistTrack.TScore = _playlistService.CalcTScore(playlistTrack);
            if (oldVote != null)
            {
                oldVote.TScore = _playlistService.CalcTScore(oldVote);
            }
            Console.WriteLine("Jeg kan nå hertil:::");
            //user.Vote.TScore += 1;
        }
    }
}
