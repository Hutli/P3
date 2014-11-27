using System;
using WebAPI;

namespace OpenPlaylistApp.Models
{
    public class User
    {
        public event Action<Venue> VenueChanged;
        public event Action<Track> VoteChanged;

        public string Id { get; set; }

        public string Name { get; set; }

        private Track _vote;

        public Track Vote { 
            get {return _vote; }
            set { _vote = value;
            VoteChanged(_vote);
            }
        }

        private Venue _venue;

        public Venue Venue {
            get { return _venue; }
            set { _venue = value;
            if(VenueChanged != null)
                VenueChanged(_venue);
            }
        }

        public User()
        {
        }
    }
}
