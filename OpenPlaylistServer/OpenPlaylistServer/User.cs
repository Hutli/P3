namespace OpenPlaylistServer {
    public class User {
        public string Id { get; private set; }

        public PlaylistTrack Vote { get; set; }

        public User(string id) {
            Id = id;
        }
    }
}
