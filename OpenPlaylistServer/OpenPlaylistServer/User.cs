namespace OpenPlaylistServer {
    public class User {
        private PlaylistTrack _vote;

        public string Id { get; private set; }

        public PlaylistTrack Vote {
            get {
                return _vote;
            }
            set {
                _vote = value;
            }
        }

        public User(string id) {
            Id = id;
        }
    }
}
