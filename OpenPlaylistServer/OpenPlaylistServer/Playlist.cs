using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;
using System.Collections.ObjectModel;
using System.Collections;

namespace OpenPlaylistServer {
    public class Playlist{
        private ObservableCollection<PlaylistTrack> _tracks;

        public Playlist(){
            _tracks = new ObservableCollection<PlaylistTrack>();
        }

        public IEnumerable<PlaylistTrack> Tracks { 
            get {
                    foreach (var item in _tracks)
                    {
                        yield return item;
                    }
                } 
        }

        private int _totalDuration;

        public int TotalDuration {
            get {
                return _totalDuration;
            }
        }

        public void AddByURI(string trackUri){
            PlaylistTrack PlaylistTrack = new PlaylistTrack(trackUri);
            _tracks.Add(PlaylistTrack);
        }

        public void RemoveByTitle(string name) {
            if(_tracks.Any(e => e.Name.Equals(name)))
                _tracks.Remove(_tracks.First(e => e.Name.Equals(name)));
        }

        public void Remove(PlaylistTrack track) {
            _tracks.Remove(track);
        }

        #region TestingPurposes
        public void MoveUp(PlaylistTrack track) {
            if(_tracks.Count == 0)
                return;
            int index = _tracks.IndexOf(track);
            if(index == 0)
                return;
            PlaylistTrack temp;
            temp = _tracks[index - 1];
            _tracks[index - 1] = track;
            _tracks[index] = temp;
        }

        public void MoveDown(PlaylistTrack track) {
            int index = _tracks.IndexOf(track);
            if(index == _tracks.Count - 1)
                return;
            PlaylistTrack temp;
            temp = _tracks[index + 1];
            _tracks[index + 1] = track;
            _tracks[index] = temp;
        }
        #endregion

        public PlaylistTrack NextTrack(List<User> users) {
            CountVotes(users);

            //Sort(_tracks);
            PlaylistTrack next = _tracks.First();
            next.ResetPScore();
            _tracks.Remove(next);
            return next;
        }

        public void Sort(List<PlaylistTrack> list) {
            
            list.Sort((x,y) => y.TScore.CompareTo(x.TScore));
        }

        public void CurrentStanding(List<User> users){
            var grouping = users.GroupBy(u => u.Vote);
            foreach (var track in grouping)
            {
                List<User> tempUsers = new List<User>();
                foreach (var user in track)
                {
                    tempUsers.Add(user);
                }
                if(_tracks.Contains(track.Key))
                    track.Key.TScore = tempUsers.Count;
            }
        }

        private void CountVotes(List<User> users) {
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
    }
}
