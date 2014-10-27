using SpotifyDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
{
    public class PlaylistService : IPlaylistService
    {
        readonly ObservableCollection<PlaylistTrack> _tracks;
        readonly ReadOnlyObservableCollection<PlaylistTrack> _roTracks;

        private IUserService _userService;

        public PlaylistService(IUserService userService){
            _tracks = new ObservableCollection<PlaylistTrack>();
            _roTracks = new ReadOnlyObservableCollection<PlaylistTrack>(_tracks);
            _userService = userService;
        }

        public PlaylistTrack FindTrack(string trackUri)
        {
            return _tracks.FirstOrDefault(x => x.Uri == trackUri);
        }

        public void AddByURI(string trackUri)
        {
            //var track = SpotifyLoggedIn.Instance.TrackFromLink(trackUri).Result;
            PlaylistTrack PlaylistTrack = new PlaylistTrack(trackUri);
            _tracks.Add(PlaylistTrack);
        }

        //public void AddByRef(playlistTrack playlistTrack)
        //{
        //    _tracks.Add(playlistTrack);
        //}

        public void RemoveByTitle(string name)
        {
            if (_tracks.Any(e => e.Name.Equals(name)))
                _tracks.Remove(_tracks.First(e => e.Name.Equals(name)));
        }

        public void Remove(PlaylistTrack track)
        {
            _tracks.Remove(track);
        }


        public ReadOnlyObservableCollection<PlaylistTrack> Tracks
        {
            get {
                return _roTracks;
            }
        }


        public PlaylistTrack NextTrack()
        {
            CountAndUpdatePVotes();
            Sort();
            PlaylistTrack next = _tracks.First();
            next.ResetPScore();
            _tracks.Remove(next);
            return next;
        }

        private void Sort()
        {
            //_tracks.Sort((x, y) => y.TScore.CompareTo(x.TScore));
        }

        private void CountAndUpdatePVotes()
        {
            var users = _userService.Users;
            // group users by which track they have voted on
            var grouping = users.GroupBy(u => u.Vote);

            foreach (var track in grouping)
            {
                int numVotesOnTrack = 0;

                // count how many users voted for each track and update the pScore (permanent score)
                foreach (var user in track)
                {
                    numVotesOnTrack += 1;
                }

                track.Key.UpdatePScore(numVotesOnTrack);
            }
        }


        public void Add(PlaylistTrack track)
        {
            _tracks.Add(track);
        }
    }
}
