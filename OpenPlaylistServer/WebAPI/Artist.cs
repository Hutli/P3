using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebAPI
{
    public class Artist : SpotifyObject
    {
        private List<string> _genres = new List<string>();

        public Artist(string id, string name) : base(id, name) { }

        public List<string> Genres { get { return new List<string>(_genres); } }

        public string Href { get { return "https://api.spotify.com/v1/artists/" + ID; } }

        public override string URI { get { return "spotify:artist:" + ID; } }

        public override string ToString() { return Name; }
    }
}