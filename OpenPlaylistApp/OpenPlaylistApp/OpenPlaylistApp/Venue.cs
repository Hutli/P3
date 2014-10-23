using System;
using System.Collections.Generic;
using System.Text;

namespace OpenPlaylistApp
{
    class Venue
    {
        private string _name;
        private string _ip;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }


        public Venue(string name, string ip) {
            _name = name;
            _ip = ip;
        }
    }
}
