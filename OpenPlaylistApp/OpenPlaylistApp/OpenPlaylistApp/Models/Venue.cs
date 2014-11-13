﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenPlaylistApp
{
    public class Venue
    {
        private string _name;
        private string _detail;
        private string _iconUrl;
        private string _ip;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        public string IconUrl
        {
            get { return _iconUrl; }
            set { _iconUrl = value; }
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