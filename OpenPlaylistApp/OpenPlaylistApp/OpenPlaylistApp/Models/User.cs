using System;
using WebAPI;

namespace OpenPlaylistApp.Models
{
    public class User
    {
        public event Action<Venue> VenueChanged;

        public string Id { get; set; }

        public string Name { get; set; }

        public Track Vote { get; set; }

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
