using WebAPILib;

namespace OpenPlaylistApp
{
    public class User
    {
        public string Id { get; private set; }

        public string Name { get; set; }

        public Track Vote { get; set; }

        public Venue Venue { get; set; }

        public User(string id)
        {
            Id = id;
        }
    }
}
