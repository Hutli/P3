using System.Linq;

namespace OpenPlaylistServer
{
    public class VoteService : IVoteService
    {
        private IPlaylistService _playlistService;
        private IUserService _userService;

        public VoteService(IPlaylistService playlistService, IUserService userService) {
            _playlistService = playlistService;
            _userService = userService;
        }

        public void Vote(string userId, string trackUri)
        {
            User user;
            
            // is playlistTrack already voted on?
            PlaylistTrack playlistTrack = _playlistService.FindTrack(trackUri);
            if (playlistTrack == null)
            {
                // playlistTrack is not already voted on, so creating new instance and adding to list
                playlistTrack = new PlaylistTrack(trackUri);
                _playlistService.Add(playlistTrack);
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
                    var oldVote = user.Vote;
                    oldVote.TScore -= 1;
                }
            }
            else
            {
                // user is not known. Adding user to list of known users
                user = new User(userId);
                _userService.Add(user);
            }

            //  set user's vote to new track
            if (user != null)
            {
                user.Vote = playlistTrack;
                user.Vote.TScore += 1;
            }
        }
    }
}
