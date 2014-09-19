using System;
using System.Collections;
using System.Collections.Generic;

namespace WebAPILib
{
	public class Artist: SpotifyObject
	{
		private List<string> Genres;

		public int Popularity { get; set; }

		public Artist (int id, string name, IEnumerable<string> genres): base(id, name)
		{
			Genres = new List<string> ();
			foreach (var item in genres) {
				Genres.Add (item);
			}
		}




	}
}

