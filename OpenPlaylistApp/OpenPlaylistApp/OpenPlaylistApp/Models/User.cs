using System;
using System.Collections.Generic;
using System.Text;
using WebAPILib;

namespace OpenPlaylistApp
{
    public class User
    {
        private string _id;
        private string _name;
        private Track _vote;
        private Venue _venue;

        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Track Vote
        {
            get { return _vote; }
            set { _vote = value; }
        }

        public Venue Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        public User(string id)
        {
            _id = id;
        }
    }
}
