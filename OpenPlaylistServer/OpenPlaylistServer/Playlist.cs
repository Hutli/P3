using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;
using System.Collections.ObjectModel;
using System.Collections;

namespace OpenPlaylistServer {
    class User {
        private int _id;
        private PTrack _vote;
        static int incre;

        public int Id {
            get {
                return _id;
            }
        }

        public PTrack Vote{
            get{return _vote;}
            set {
                _vote = value;
            }
        }

        public User() {
            _id = incre++;
        }
    }

    class PTrack 
        //: Track kan vi lave ned arvning?
    {
        private int _pScore;
        private string _name;
        private int _duration;
        private Track _track;

        public void UpdatePScore(List<User> users) {
            _pScore += users.Count;
        }

        public int PScore {
            get {
                return _pScore;
            }
        }

        public string Name {
            get {
                return _name;
            }
        }

        public int Duration {
            get {
                return _duration; //Maybe to seconds idk??
            }
        }

        public Track Track{
            get{return _track;}
        }
        
        public PTrack(Track track){
            _track = track;
            _name = track.Name;
            _duration = track.Duration;
        }
    }

    class Playlist{
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
            PTrack track = new PTrack(SpotifyLoggedIn.Instance.TrackFromLink(trackId));
            _tracks.Add(track);
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

        public PTrack NextTrack(List<User> users) {
            CountVotes(users);
            Sort(_tracks);
            PTrack next = _tracks.First();
            _tracks.Remove(next);
            return next;
        }

        public void Sort(List<PTrack> list) {
            list.OrderBy(x => x.PScore);
        }

        public List<PTrack> currentStanding(List<User> users){
            List<List<User>> userList = new List<List<User>>();
            foreach(User user in users) {
                List<User> f = userList.Find(l => l.Any(u => u.Vote.Equals(user.Vote)));
                if(f == null){
                    List<User> newList = new List<User>();
                    newList.Add(user);
                    userList.Add(newList);
                }else{
                    f.Add(user);
                }
            }

            List<PTrack> returnList = new List<PTrack>();
            foreach(PTrack track in _tracks) {
                returnList.Add(track);
            }

            foreach(List<User> u in userList){
                returnList.First(e => e.Equals(u[0].Vote)).UpdatePScore(users);
            }

            Sort(returnList);

            return returnList;
        }

        private void CountVotes(List<User> users) {
            List<List<User>> userList = new List<List<User>>();
            foreach(User user in users) {
                List<User> f = userList.Find(l => l.Any(u => u.Vote.Equals(user.Vote)));
                if(f == null){
                    List<User> newList = new List<User>();
                    newList.Add(user);
                    userList.Add(newList);
                }else{
                    f.Add(user);
                }
                user.Vote = null;
            }

            foreach(List<User> u in userList){
                _tracks.First(e => e.Equals(u[0].Vote)).UpdatePScore(users);
            }
        }
    }
}
