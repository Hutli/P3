using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebAPILib {
	public class Album : SpotifyObject {
		private string _albumType;
		private List<Image> _images;

		public Album (string id, string name, string albumType, IEnumerable<Image> images) : base (id, name) {
			_id = id;
			_name = name;
			_albumType = albumType;
			_images = new List<Image> (images);
		}

		public string AlbumType{ get { return _albumType; } }

		public List<Image> Images{ get { return new List<Image> (_images); } }

		public override string URI{ get { return "spotify:album:" + ID; } }
	}
}