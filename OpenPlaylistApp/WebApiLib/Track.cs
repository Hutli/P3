using System.Collections.Generic;

namespace WebAPILib
{
    public class Track : SpotifyObject
    {
        public Track(string id, string name, int duration, bool isExplicit, int trackNumber, Search searchResult, Album album, List<Artist> artists)
            : base(id, name, searchResult)
        {
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
            Album = album;
            Album.AddTrack(this);
            Album.AddArtists(artists);
        }

        public int Duration { get; private set; }

        public bool IsExplicit { get; private set; }

        public int TrackNumber { get; private set; }

        public Album Album { get; private set; }

        public override string URI { get { return "spotify:track:" + ID; } }

        public override string ToString()
        {
            return string.Format("{0} on {1}", Name, Album);
        }
    }
}

