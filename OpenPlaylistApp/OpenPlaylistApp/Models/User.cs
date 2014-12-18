using System;
using WebAPI;

namespace OpenPlaylistApp.Models
{
    public class User
    {
        private Venue _venue;
        private Track _vote;
        public double ScreenWidth { get; set; }
        public double ScreenHeight { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public Track Vote
        {
            get { return _vote; }
            set
            {
                _vote = value;
                VoteChanged(_vote);
            }
        }

        public Venue Venue
        {
            get { return _venue; }
            set
            {
                _venue = value;
                if(VenueChanged != null)
                    VenueChanged(_venue);
            }
        }

        public event Action<Venue> VenueChanged;
        public event Action<Track> VoteChanged;
    }
}