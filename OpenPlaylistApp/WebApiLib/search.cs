using System;
using System.Collections.Generic;
using WebApiLib;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApiLib {
	public enum SearchType {
		ALL,
		ARTIST,
		ALBUM,
		TRACK

	}

	public class Search {
		private List<Artist> _artists = new List<Artist> ();
		private List<Album> _albums = new List<Album> ();
		private List<Track> _tracks = new List<Track> ();

		public List<Artist> Artists { get { return _artists; } }

		public List<Album> Albums { get { return _albums; } }

		public List<Track> Tracks { get { return _tracks; } }

		public void AddArtist (Artist artist) {
			if (!_artists.Exists (a => a.ID == artist.ID))
				_artists.Add (artist);
		}

		public void AddAlbum (Album album) {
			if (!_albums.Exists (a => a.ID == album.ID))
				_albums.Add (album);
		}

		public void AddTrack (Track track) {
			if (!_tracks.Exists (a => a.ID == track.ID))
				_tracks.Add (track);
		}

		public readonly string searchString;

		public Search (string searchString)
			: this (searchString, SearchType.ALL) {
		}

		public Search (string searchString, SearchType type) {
			switch (type) {
			case SearchType.ALL:
			case SearchType.ARTIST:
                JObject oArtists = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=artist");
				GetArtists (oArtists ["artists"] ["items"]);
				if (type == SearchType.ALL)
					goto case SearchType.ALBUM; //TODO FIX C#!
				break;
			case SearchType.ALBUM:
                JObject oAlbums = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=album");
				GetAlbums (oAlbums ["albums"] ["items"]);
				if (type == SearchType.ALL)
					goto case SearchType.TRACK; //TODO FIX C#!
				break;
			case SearchType.TRACK:
                JObject oTracks = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=track");
				GetTracks (oTracks ["tracks"] ["items"]);
				break;
			}
		}

		private List<Artist> GetArtists (JToken jsonCode) {
			List<Artist> artists = new List<Artist> ();
			foreach (JObject jsonArtist in jsonCode) {
				string id = (string)(jsonArtist ["id"]);
				string name = (string)(jsonArtist ["name"]);
				if (_artists.Exists (a => a.ID.Equals (id))) {
					artists.Add (_artists.Find (a => a.ID.Equals (id))); 
				} else {
					Artist artist = new Artist (id, name, this);
					artists.Add (artist);
					AddArtist (artist);
				}
			}
			return artists;
		}

		private List<Album> GetAlbums (JToken jsonCode) {
			List<Album> albums = new List<Album> ();
			foreach (JObject jsonAlbum in jsonCode) {
				string id = (string)(jsonAlbum ["id"]);
				string name = (string)(jsonAlbum ["name"]);
				string albumType = (string)(jsonAlbum ["album_type"]);
				List<Image> images = GetImages (jsonAlbum);
				if (_albums.Exists (a => a.ID.Equals (id))) {
					albums.Add (_albums.Find (a => a.ID.Equals (id))); 
				} else {
					Album album = new Album (id, name, albumType, images, this);
					albums.Add (album);
					AddAlbum (album);
				}
			}
			return albums;
		}

		private List<Track> GetTracks (JToken jsonCode) {
			List<Track> tracks = new List<Track> ();
			foreach (JObject jsonTrack in jsonCode) {
				string id = (string)(jsonTrack ["id"]);
				if (!_tracks.Exists (a => a.ID.Equals (id))) {
					string name = (string)(jsonTrack ["name"]);
					int popularity = (int)(jsonTrack ["popularity"]);
					int duration = (int)(jsonTrack ["duration_ms"]);
					bool isExplicit = (bool)jsonTrack ["explicit"];
					int trackNumber = (int)(jsonTrack ["track_number"]);

					List<Artist> artists = GetArtists (jsonTrack ["artists"]);

					List<Image> images = GetImages (jsonTrack ["album"].ToObject<JObject> ());

					Album album = null;
					string albumID = (string)jsonTrack ["album"] ["id"];
					if (!Albums.Exists (a => a.ID.Equals (albumID))) {
						string albumName = (string)jsonTrack ["album"] ["name"];
						string albumType = (string)jsonTrack ["album"] ["album_type"];
						album = new Album (albumID, albumName, albumType, images, this, artists);
						AddAlbum (album);
					} else {
						album = Albums.Find (a => a.ID.Equals (albumID));
					}

					Track track = new Track (id, name, popularity, duration, isExplicit, trackNumber, album, this, artists);

					tracks.Add (track);
					AddTrack (track);
				}
			}
			return tracks;
		}

		private List<Image> GetImages (JObject imageList) {
			List<Image> images = new List<Image> ();
			foreach (JObject image in imageList["images"]) {
				int height = (int)(image ["height"]);
				int width = (int)(image ["width"]);
				string imageUrl = (string)(image ["url"]);
				images.Add (new Image (height, width, imageUrl));
			}
			return images;
		}

		public static JObject getJobject (string url) {
			string str = Request.get (url);
			JObject o = JObject.Parse (str);
			return o;
		}
	}
}
