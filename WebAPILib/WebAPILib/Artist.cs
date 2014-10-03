using System;
using System.Collections;
using System.Collections.Generic;

namespace WebAPILib {
	public class Artist: SpotifyObject {
		private List<string> _genres;

		public int Popularity { get; set; }

		public Artist (string id, string name, IEnumerable<string> genres) : base (id, name) {
			_genres = new List<string> (genres);

		}

		public List<string> Genres{ get { return new List<string> (_genres); } }

		public override string URI{ get { return "spotify:artist:" + ID; } }
	}
}