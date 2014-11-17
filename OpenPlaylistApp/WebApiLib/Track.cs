using System.Collections.Generic;

namespace WebAPILib {
	public class Track : SpotifyObject {
        public Track(string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Search searchResult)
            : base(id, name, searchResult)
        {
            Popularity = popularity;
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
        }

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Search searchResult, Album album)
            : this(id, name, popularity, duration, isExplicit, trackNumber, searchResult)
        {
            album.AddTrack(this);
			Album = album;
		}

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Search searchResult, Album album, List<Artist> artists)
			: this (id, name, popularity, duration, isExplicit, trackNumber, searchResult, album) {
			album.AddArtists (artists);
		}

	    public int Popularity { get; private set; }

	    public int Duration { get; private set; }

	    public bool IsExplicit { get; private set; }

	    public int TrackNumber { get; private set; }

	    public Album Album { get; private set; }

		public override string URI{ get { return "spotify:track:" + ID; } }

        public override string ToString()
        {
            return string.Format("{0} on {1}", Name, Album);
        }
	}
}

