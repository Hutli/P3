using System;
using System.Collections.Generic;

namespace WebAPILib
{
	public class Track: SpotifyObject
	{
		public int Popularity { get; set; }
		public int Duration { get; set; }
		public bool Explicit { get; set; }
		public int TrackNumber { get; set; }

		public Track (int id, string name): base(id, name)
		{
			
		}

	}
}

