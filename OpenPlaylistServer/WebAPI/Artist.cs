using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Artist : SpotifyObject
    {
        private List<string> _genres = new List<string>();

        public Artist(string id, string name, List<string> genres) : base(id, name) {
            _genres = new List<string>(genres);
        }

        public List<string> Genres { get { return new List<string>(_genres); } }

        public string Href { get { return "https://api.spotify.com/v1/artists/" + ID; } }

        public override string URI { get { return "spotify:artist:" + ID; } }

        public override string ToString() { return Name; }
    }
}