using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
    public class Album : SpotifyObject {
        private string _albumType;
        private List<Image> _images = new List<Image>();
        private List<Artist> _artists = new List<Artist>();
        private List<Track> _tracks = new List<Track>();
		private bool tracksCached = false;
		private bool artistsCached = false;

        public string AlbumType { get { return _albumType; } }

        public List<Image> Images { get { return new List<Image>(_images); } }

        public List<Artist> Artists {
            get {
				if (!artistsCached)
					cache ();
                return new List<Artist>(_artists);
            }
        }

		public List<Track> Tracks { 
			get {
				if (!tracksCached)
					cache ();
				return new List<Track>(_tracks);
			} 
		}

		public string Href{ get { return "https://api.spotify.com/v1/albums/" + ID; } }

		private void cache(){
			JObject o = search.getJobject(Href);
			if (!artistsCached) {
				List<Artist> artists = new List<Artist> ();
				foreach (JObject jsonArtist in o["artists"]) {
					string id = Convert.ToString (jsonArtist ["id"]);
					string name = Convert.ToString (jsonArtist ["name"]);
					if (SearchResult.Artists.Exists (a => id.Equals (a.ID))) {
						SearchResult.Artists.Find (a => id.Equals (a.ID)).addAlbum (this); 
						artists.Add (SearchResult.Artists.Find (a => id.Equals (a.ID)));
					} else {
						Artist tmpArtist = new Artist (id, name, SearchResult, new List<Album> { this });
						SearchResult.addArtist (tmpArtist);
						artists.Add (tmpArtist);
					}
				}
				_artists = artists;
				artistsCached = true;
			}
			if (!tracksCached) {
				List<Track> tracks = new List<Track> ();
				foreach (JObject jsonTrack in o["tracks"]["items"]) {
					string id = (string)(jsonTrack ["id"]);
					string name = (string)(jsonTrack ["name"]);
					int duration = (int)(jsonTrack ["duration_ms"]);
					bool isExplicit = (bool)jsonTrack ["explicit"];
					int trackNumber = (int)(jsonTrack ["track_number"]);
					if (SearchResult.Tracks.Exists (a => id.Equals (a.ID)))
						tracks.Add (SearchResult.Tracks.Find (a => id.Equals (a.ID)));
					else {
						Track tmpTrack = new Track (id, name, 0, duration, isExplicit, trackNumber, this, SearchResult); //TODO Spotify don't want to tell ud popularity
						SearchResult.addTrack (tmpTrack);
						tracks.Add (tmpTrack);
					}
				}
				_tracks = tracks;
				tracksCached = true;
			}
		}

        public override string URI { get { return "spotify:album:" + ID; } }

		public Album(string id, string name, string albumtype, IEnumerable<Image> images, search searchResult, List<Artist> artists) : this(id,name,albumtype,images,searchResult){
			_artists = artists;
			foreach (Artist a in artists) {
				a.addAlbum (this);
			}
		}

		public Album(string id, string name, string albumtype, IEnumerable<Image> images, search searchResult) : base(id, name, searchResult) {
            _albumType = albumtype;
            _images = new List<Image>(images);
        }

		public void addArtists(List<Artist> artists){
			if (_artists.Count == 0) {
				_artists = new List<Artist> (artists);
				foreach (Artist a in artists) {
					a.addAlbum (this);
				}
				artistsCached = true;
			}
		}

        public void addTrack(Track track) {
            if(_tracks.Exists(a => track.ID.Equals(a.ID)))
                throw new Exception(); //TODO Create spotify exception
            _tracks.Add(track);
        }
    }
}