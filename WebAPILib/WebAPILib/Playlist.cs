using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebAPILib {
	public class Playlist {
		private List<Image> _images;
		private List<Track> _tracks;

		public Playlist () {
		}

		public List<Image> Images{ get { return new List<Image> (_images); } }

		public List<Track> Tracks{ get { return new List<Track> (_tracks); } }

		public void addTrack (Track track) {
			_tracks.Add (track); //TODO Should it be possible to add the same song twice?
		}

		public void removeTrack (Track track) {
			if (_tracks.Contains (track))
				_tracks.Remove (track);
			else
				throw new Exception (); //TODO Make spotify exception
		}
	}
}

