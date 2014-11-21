using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Track : SpotifyObject
    {
        public Track(string id, string name, int duration, bool isExplicit, int trackNumber, Album album)
            : base(id, name)
        {
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
            Album = album;
        }

        public int Duration { get; private set; }

        public bool IsExplicit { get; private set; }

        public int TrackNumber { get; private set; }

        [JsonProperty]
        public Album Album { get; private set; }

        public override string URI { get { return "spotify:track:" + ID; } }

        public override string ToString() { return string.Format("{0} on {1}", Name, Album); }
    }
}

