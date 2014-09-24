using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
	public class Album : SpotifyObject {
		private string _albumType = null;
		private List<Image> _images = null;
		private List<Artist> _artists = null;
		private List<Track> _tracks = null;

		public string AlbumType{ get { return _albumType; } }

		public List<Image> Images{ get { return new List<Image> (_images); } }

		public List<Artist> Artists{ 
			get {
				if (_artists != null) {
					string href = "https://api.spotify.com/v1/albums/" + ID;
					JObject o = search.get(href);
					List<Artist> artists = new List<Artist> ();
					foreach (JObject artist in o["artists"]["items"]) {
						string id = Convert.ToString (artist ["id"]);
						string name = Convert.ToString (artist ["name"]);
						/*if (SearchResult.Artists.Contains (a => a.ID == id)) {
							SearchResult.Artists.Find (a => a.ID == id).addAlbum (this);
							artists.Add (SearchResult.Artists.Find (a => a.ID == id));
						} else {
							Artist tmpArtist = new Artist (id, name, new List<Album> (this));
							SearchResult.addArtist (tmpArtist);
							artists.Add (tmpArtist);
						}*/
					}
					_artists = artists;
				}
				return new List<Artist> (_artists); 
			}
		}

		public List<Track> tracks{ get { return new List<Track> (_tracks); } }

		public override string URI{ get { return "spotify:album:" + ID; } }

		public Album (string id, string name, string albumtype, IEnumerable<Image> images) : base (id, name) {
			_albumType = albumtype;
			_images = new List<Image> (images);
		}

		public void addTrack(Track track)
		{
			if (_tracks.Exists(a => track.ID == a.ID))
				throw new Exception (); //TODO Create spotify exception
			_tracks.Add (track);
		}
	}
}