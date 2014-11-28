using System.Linq;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation
{
    public class PlaylistService : IPlaylistService
    {
        readonly ConcurrentBagify<Track> _tracks;

        private readonly IUserService _userService;

        public PlaylistService(IUserService userService){
            _tracks = new ConcurrentBagify<Track>();
            _userService = userService;
        }

        public Track FindTrack(string trackUri)
        {
            return _tracks.FirstOrDefault(x => x.URI == trackUri);
        }

        public  ConcurrentBagify<Track> Tracks
        {
            get {
                return _tracks;
            }
        }

        public int CalcTScore(Track track)
        {
            return _userService.Users.Count(u => u.Vote == track);
        }

        private void ResetVotes(Track track)
        {
            var users = _userService.Users.Where(u => u.Vote == track);
            foreach (var user in users)
            {
                user.Vote = null;
            }

            var tScore = CalcTScore(track);
            track.TScore = tScore;
        }

        public Track NextTrack()
        {
            CountAndUpdatePVotes();
            Track next = _tracks.OrderByDescending(x => x.TotalScore).FirstOrDefault();

            if (next == null) return null;
            next.PScore = 0;
            ResetVotes(next);

            return next;
        }

        private void CountAndUpdatePVotes()
        {
            foreach (var track in _tracks)
            {
                var tScore = CalcTScore(track);
                track.TScore = tScore;
                // add temp score to permanent score
                track.PScore += track.TScore;
            }
        }

        public void Add(Track track)
        {
            _tracks.Add(track);
        }
    }
}
