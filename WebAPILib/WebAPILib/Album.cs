using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebAPILib {
	public class Album : SpotifyObject {
		private string _albumType = null;
		private List<Image> _images = null;
		private List<Artist> _artists = null;
		private List<Track> _tracks = null;

		public string AlbumType{ get { return _albumType; } }

		public List<Image> Images{ get { return new List<Image> (_images); } }

		public List<Artist> Artists{ get { return new List<Artist> (_artists); } }

		public List<Track> tracks{ get { return new List<Track> (_tracks); } }

		public override string URI{ get { return "spotify:album:" + ID; } }

		public Album (string id, string name, IEnumerable<Artist> artists) : base (id, name) {
			_artists = new List<Artist>(artists);
		}

		public Album (string id, string name, IEnumerable<Artist> artists, string albumtype, IEnumerable<Image> images, IEnumerable<Track> tracks) : this (id, name, artists) {
			_albumType = albumtype;
			_images = new List<Image> (images);
			_tracks = new List<Track> (tracks);
		}

	}
}