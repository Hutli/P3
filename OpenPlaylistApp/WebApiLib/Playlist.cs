using System;
using System.Collections.Generic;

namespace WebAPILib {
	public class Playlist : SpotifyObject {
		private List<Track> _tracks;

		public Playlist (string id, string name, Search searchResult) : base(id, name, searchResult) {}
			
		public List<Track> Tracks{ get { return new List<Track> (_tracks); } }

        /// <summary>
        /// Adds track to the playlist
        /// </summary>
        /// <param name="track">Track to be added</param>
		public void AddTrack (Track track) {
			if (!_tracks.Exists (t => t.ID == track.ID)) // No duplicates
				_tracks.Add (track); //TODO Vote when the track is already there
		}

        /// <summary>
        /// Removes track from playlist
        /// </summary>
        /// <param name="track">Track to be removed</param>
		public void RemoveTrack (Track track) {
			if (_tracks.Contains (track))
				_tracks.Remove (track);
			else
				throw new Exception (); //TODO Make spotify exception
		}
	}
}

