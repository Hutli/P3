using System.Collections.Generic;

namespace WebAPILib {
	public class Track : SpotifyObject {
		private int _popularity;
		private int _duration;
		private bool _isExplicit;
		private int _trackNumber;
		private Album _album = null;

        public Track(string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Search searchResult)
            : base(id, name, searchResult)
        {
            _popularity = popularity;
            _duration = duration;
            _isExplicit = isExplicit;
            _trackNumber = trackNumber;
        }

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Search searchResult, Album album)
            : this(id, name, popularity, duration, isExplicit, trackNumber, searchResult)
        {
            album.AddTrack(this);
			_album = album;
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

