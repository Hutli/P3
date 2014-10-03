using System;
using System.Collections.Generic;

namespace WebAPILib {
	public class Track : SpotifyObject {
		private int _popularity;
		private int _duration;
		private bool _isExplicit;
		private int _trackNumber;
		private Album _album = null;

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Album album, search searchResult) : base (id, name, searchResult) {
			_popularity = popularity;
			_duration = duration;
			_isExplicit = isExplicit;
			_trackNumber = trackNumber;
			addAlbum (album);
		}

		public Track (string id, string name, int popularity, int duration, bool isExplicit, int trackNumber, Album album, search searchResult, List<Artist> artists)
			: this (id, name, popularity, duration, isExplicit, trackNumber, album, searchResult) {
			List<Artist> newArtists = new List<Artist> ();
			foreach (Artist a in artists) {
				if (SearchResult.Artists.Exists (b => a.ID.Equals (b.ID))) {
					SearchResult.Artists.Find (b => a.ID.Equals (b.ID)).addAlbum (album);
					newArtists.Add (SearchResult.Artists.Find (b => a.ID.Equals (b.ID)));
				} else {
					SearchResult.addArtist (a);
					newArtists.Add (a);
				}
			}
			album.addArtists (newArtists);
			addAlbum (album);
		}

		public void addAlbum (Album album) {
			if (Album == null) {
				_album = album;
				album.addTrack (this);
			}
		}

		public int Popularity{ get { return _popularity; } }

		public int Duration{ get { return _duration; } }

		public bool IsExplicit{ get { return _isExplicit; } }

		public int TrackNumber{ get { return _trackNumber; } }

		public Album Album { get { return _album; } }

		public override string URI{ get { return "spotify:track:" + ID; } }
	}
}

