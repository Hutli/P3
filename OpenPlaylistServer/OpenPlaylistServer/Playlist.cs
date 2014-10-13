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

        public void Add(string trackId){
            PTrack track = new PTrack(SpotifyLoggedIn.Instance.TrackFromLink(trackId));
            _tracks.Add(track);
        }

        public void RemoveByTitle(string name) {
            _tracks.Remove(_tracks.Where(e => e.Track.Name.Equals(name)).First());
        }

        public void Remove(PTrack track) {
            _tracks.Remove(track);
        }

        public void MoveUp(PTrack track) {
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

        public void CountVotes(List<User> users) {
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

            foreach(List<User> u in userList){
                _tracks.Where(e => e.Equals(u[0].Vote)).First().UpdatePScore(users);
            }
        }
    }
}
