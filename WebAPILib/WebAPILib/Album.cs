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

		public Album (string id, string name, string albumtype, IEnumerable<Image> images) : base (id, name) {
			_albumType = albumtype;
			_images = new List<Image> (images);
		}

		public Album (string id, string name, string albumtype, IEnumerable<Image> images, IEnumerable<Artist> artists) : this (id, name, albumtype, images) {
			_artists = new List<Artist>(artists);
		}

	}
}