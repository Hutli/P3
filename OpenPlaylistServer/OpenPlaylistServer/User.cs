using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer {
    public class User {
        private string _id;
        private PlaylistTrack _vote;

        public string Id {
            get {
                return _id;
            }
        }

        public PlaylistTrack Vote {
            get {
                return _vote;
            }
            set {
                _vote = value;
            }
        }

        public User(string id) {
            _id = id;
        }

        //public User(string id, playlistTrack vote) {
        //    _id = id;
        //    _vote = vote;
        //}
    }
}
