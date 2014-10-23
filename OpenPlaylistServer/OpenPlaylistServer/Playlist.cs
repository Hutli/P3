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
        public List<PTrack> _tracks;

        public Playlist(){
            _tracks = new List<PTrack>();
        }


        private int _totalDuration;

        public int TotalDuration {
            get {
                return _totalDuration;
            }
        }

        public void AddByURI(string trackId){
            var track = SpotifyLoggedIn.Instance.TrackFromLink(trackId).Result;
            PTrack ptrack = new PTrack(track);
            _tracks.Add(ptrack);
        }

        public void AddByRef(Track track) {
            PTrack pTrack = new PTrack(track);
            _tracks.Add(pTrack);
        }

        public void RemoveByTitle(string name) {
            if(_tracks.Any(e => e.Name.Equals(name)))
                _tracks.Remove(_tracks.First(e => e.Track.Name.Equals(name)));
        }

        public void Remove(PTrack track) {
            _tracks.Remove(track);
        }

        #region TestingPurposes
        public void MoveUp(PTrack track) {
            if(_tracks.Count == 0)
                return;
            int index = _tracks.IndexOf(track);
            if(index == 0)
                return;
            PTrack temp;
            temp = _tracks[index - 1];
            _tracks[index - 1] = track;
            _tracks[index] = temp;
        }

        public void MoveDown(PTrack track) {
            int index = _tracks.IndexOf(track);
            if(index == _tracks.Count - 1)
                return;
            PTrack temp;
            temp = _tracks[index + 1];
            _tracks[index + 1] = track;
            _tracks[index] = temp;
        }
        #endregion

        public PTrack NextTrack(List<User> users) {
            CountVotes(users);

            Sort(_tracks);
            PTrack next = _tracks.First();
            next.ResetPScore();
            _tracks.Remove(next);
            return next;
        }

        public void Sort(List<PTrack> list) {
            
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
            var grouping = users.GroupBy(u => u.Vote);
            foreach (var track in grouping)
            {
                List<User> tempUsers = new List<User>();
                foreach (var user in track)
                {
                    tempUsers.Add(user);
                }
                if (_tracks.Contains(track.Key))
                    track.Key.UpdatePScore(tempUsers);
            }
        }
    }
}
