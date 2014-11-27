using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Track : SpotifyObject
    {
        [JsonConstructor]
        public Track(string id, string name, int duration, bool isExplicit, int trackNumber, string isrc, string previewURL, Album album)
            : base(id, name)
        {
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
            ISRC = isrc;
            PreviewURL = previewURL;
            Album = album;
            IsFiltered = false;
        }

        public Track()
        {
            
        }

        public string ISRC { get; set; }

        public bool IsFiltered;

        public string PreviewURL { get; set; }

        public int Duration { get; set; }

        public int CurrentDurationStep { get; set; }

        public bool IsExplicit { get; set; }

        public int TrackNumber { get; set; }

        [JsonProperty]
        public Album Album { get; set; }

        public override string URI { get { return "spotify:track:" + Id; } }

        public override string ToString() { return string.Format("{0} on {1}", Name, Album); }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Track))
            {
                return ((Track)obj).Id == Id || ((Track)obj).ISRC == ISRC;
            }
            return false;
        }
    }
}

