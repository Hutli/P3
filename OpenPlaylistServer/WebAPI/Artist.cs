using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Artist : SpotifyObject
    {
        public Artist(string id, string name) : base(id, name) { }


        public string Href { get { return "https://api.spotify.com/v1/artists/" + Id; } }

        public override string URI { get { return "spotify:artist:" + Id; } }

        public override string ToString() { return Name; }
    }
}