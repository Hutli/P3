using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebAPILib {
	public class Album : SpotifyObject {
		private string _albumType;
		private List<Image> _images;
		private List<Artist> _artists;
		private List<Track> _tracks;
		private bool _addingArtist;

		public string AlbumType{ get { return _albumType; } }

		public List<Image> Images{ get { return new List<Image> (_images); } }

		public List<Artist> Artists{ get { return new List<Artist> (_artists); } }

		public List<Track> tracks{ get { return new List<Track> (_tracks); } }

		public override string URI{ get { return "spotify:album:" + ID; } }

		public Album (string id, string name, string albumType, IEnumerable<Image> images) : base (id, name) {
			_id = id;
			_name = name;
			_albumType = albumType;
			_images = new List<Image> (images);
			_artists = new List<Artist> ();
		}

	}
}