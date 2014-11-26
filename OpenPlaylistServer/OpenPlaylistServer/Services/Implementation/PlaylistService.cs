using System.Linq;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Services.Implementation
{
    public class PlaylistService : IPlaylistService
    {
        readonly ConcurrentBagify<PlaylistTrack> _tracks;

        private IUserService _userService;

        public PlaylistService(IUserService userService){
            _tracks = new ConcurrentBagify<PlaylistTrack>();
            //_roTracks = new ConcurrentBag<PlaylistTrack>(_tracks);
            _userService = userService;
        }

        public PlaylistTrack FindTrack(string trackUri)
        {
            return _tracks.FirstOrDefault(x => x.URI == trackUri);
        }

        public void AddByURI(string trackUri)
        {
            //var track = SpotifyLoggedIn.Instance.TrackFromLink(trackUri).Result;
            PlaylistTrack playlistTrack = new PlaylistTrack(trackUri);
            _tracks.Add(playlistTrack);
        }

        //public void AddByRef(playlistTrack playlistTrack)
        //{
        //    _tracks.Add(playlistTrack);
        //}


        public  ConcurrentBagify<PlaylistTrack> Tracks
        {
            get {
                return _tracks;
            }
        }

        public int CalcTScore(PlaylistTrack track)
        {
            return _userService.Users.Count(u => u.Vote == track);
        }

        public void ResetVotes(PlaylistTrack track)
        {
            var users = _userService.Users.Where(u => u.Vote == track);
            foreach (var user in users)
            {
                user.Vote = null;
            }

            var tScore = CalcTScore(track);
            track.TScore = tScore;
        }

        public PlaylistTrack NextTrack()
        {
            CountAndUpdatePVotes();
            PlaylistTrack next = _tracks.OrderByDescending(x => x.TotalScore).FirstOrDefault();

            if (next == null) return null;
            next.ResetPScore();
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
                track.UpdatePScore(track.TScore);
                
            }
        }


        public void Add(PlaylistTrack track)
        {
            _tracks.Add(track);
        }
    }
}
