using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;

namespace OpenPlaylistServer {
    public class PTrack {
        private int _pScore = 0;
        private string _name;
        private int _duration;
        private Track _track;
        private int _tScore = 0;

        public int TScore {
            get {
                return _tScore;
            }
            set {
                _tScore = value;
            }
        }

        public void UpdatePScore(List<User> users) {
            _pScore += users.Count;
        }

        public void ResetPScore() {
            _pScore = 0;
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

        public Track Track {
            get {
                return _track;
            }
        }

        public PTrack() {
        }

        public PTrack(Track track) {
            _track = track;
            _name = track.Name;
            _duration = track.Duration;
        }

        public override string ToString() {
            return _name;
        }
    }
}
