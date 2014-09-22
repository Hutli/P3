using System;
using System.Collections;
using System.Collections.Generic;

namespace WebAPILib {
	public class Artist: SpotifyObject {
		private List<string> _genres;
		private List<Album> _albums;
		private bool _addingAlbum;

		public int Popularity { get; set; }

		public Artist (string id, string name, IEnumerable<string> genres) : base (id, name) {
			_genres = new List<string> (genres);
			_albums = new List<Album> ();
		}

		public Artist (string id, string name, IEnumerable<string> genres, IEnumerable<Album> albums) : this (id, name, genres) {
			foreach (var item in albums) {
			}
		}

		public List<string> Genres{ get { return new List<string> (_genres); } }

		public List<Album> Albums { get { return new List<Album> (_albums); } }

		public override string URI{ get { return "spotify:artist:" + ID; } }

	}
}