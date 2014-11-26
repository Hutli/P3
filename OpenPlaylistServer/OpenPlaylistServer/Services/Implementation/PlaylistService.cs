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
            return _tracks.FirstOrDefault(x => x.Uri == trackUri);
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


        public PlaylistTrack NextTrack()
        {
            CountAndUpdatePVotes();
            PlaylistTrack next = _tracks.OrderByDescending(x => x.TotalScore).FirstOrDefault();

            if (next == null) return null;
            next.ResetPScore();

            return next;
        }

        private void CountAndUpdatePVotes()
        {
            foreach (var track in _tracks)
            {
                // add temp score to permanent score
                track.UpdatePScore(track.TScore);
                // reset temp score
                track.TScore = 0;
            }
        }


        public void Add(PlaylistTrack track)
        {
            _tracks.Add(track);
        }
    }
}
