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
		private bool _locked;

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

		public Album (string id, string name, string albumType, IEnumerable<Image> images, IEnumerable<Artist> artists) : this (id, name, albumType, images) {
			foreach (var item in artists) {
				addArtist(item);
			}
			setLock ();
		}

		public void addArtist(Artist artist)
		{
			if (_locked)
				throw new Exception ("class is locked");

			if (_addingArtist)
				return;

			_addingArtist = true;
			_artists.Add (artist);

			artist.addAlbum (this);

			_addingArtist = false;
		}	

		public void setLock()
		{
			if (_locked)
				throw new Exception ("allready locked"); //TODO fix execption

			_locked = true;
		}

	}
}