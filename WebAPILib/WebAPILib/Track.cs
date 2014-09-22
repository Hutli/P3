using System;
using System.Collections.Generic;

namespace WebAPILib {
	public class Track: SpotifyObject {
		private int _popularity;
		private int _duration;
		private bool _isExplicit;
		private int _trackNumber;
		private Album _album;

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Album album) : base (id, name) {
			_popularity = popularity;
			_duration = duration;
			_isExplicit = isExplicit;
			_trackNumber = trackNumber;
			_album = album;
		}

		public int Popularity{ get { return _popularity; } }

		public int Duration{ get { return _duration; } }

		public bool IsExplicit{ get { return _isExplicit; } }

		public int TrackNumber{ get { return _trackNumber; } }

		public Album Album { get { return _album; } }

		public override string URI{ get { return "spotify:track:" + ID; } }
	}
}

