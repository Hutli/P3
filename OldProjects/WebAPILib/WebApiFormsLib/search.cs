using System;
using System.Collections.Generic;
using WebAPILib;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
	public enum SearchType {
		ALL,
		ARTIST,
		ALBUM,
		TRACK

	}

	public class search {
		private List<Artist> _artists = new List<Artist> ();
		private List<Album> _albums = new List<Album> ();
		private List<Track> _tracks = new List<Track> ();

		public List<Artist> Artists { get { return _artists; } }

		public List<Album> Albums { get { return _albums; } }

		public List<Track> Tracks { get { return _tracks; } }

		public void addArtist (Artist artist) {
			if (!_artists.Exists (a => a.ID == artist.ID))
				_artists.Add (artist);
		}

		public void addAlbum (Album album) {
			if (!_albums.Exists (a => a.ID == album.ID))
				_albums.Add (album);
		}

		public void addTrack (Track track) {
			if (!_tracks.Exists (a => a.ID == track.ID))
				_tracks.Add (track);
		}

		public readonly string searchString;

		public search (string searchString)
			: this (searchString, SearchType.ALL) {
		}

		public search (string searchString, SearchType type) {
			switch (type) {
			case SearchType.ALL:
			case SearchType.ARTIST:
                JObject oArtists = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=artist");
				getArtists (oArtists ["artists"] ["items"]);
				if (type == SearchType.ALL)
					goto case SearchType.ALBUM; //TODO FIX C#!
				break;
			case SearchType.ALBUM:
                JObject oAlbums = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=album");
				getAlbums (oAlbums ["albums"] ["items"]);
				if (type == SearchType.ALL)
					goto case SearchType.TRACK; //TODO FIX C#!
				break;
			case SearchType.TRACK:
                JObject oTracks = getJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=track");
				getTracks (oTracks ["tracks"] ["items"]);
				break;
			}
		}

		private List<Artist> getArtists (JToken jsonCode) {
			List<Artist> artists = new List<Artist> ();
			foreach (JObject jsonArtist in jsonCode) {
				string id = (string)(jsonArtist ["id"]);
				string name = (string)(jsonArtist ["name"]);
				if (_artists.Exists (a => a.ID.Equals (id))) {
					artists.Add (_artists.Find (a => a.ID.Equals (id))); 
				} else {
					Artist artist = new Artist (id, name, this);
					artists.Add (artist);
					addArtist (artist);
				}
			}
			return artists;
		}

		private List<Album> getAlbums (JToken jsonCode) {
			List<Album> albums = new List<Album> ();
			foreach (JObject jsonAlbum in jsonCode) {
				string id = (string)(jsonAlbum ["id"]);
				string name = (string)(jsonAlbum ["name"]);
				string albumType = (string)(jsonAlbum ["album_type"]);
				List<Image> images = getImages (jsonAlbum);
				if (_albums.Exists (a => a.ID.Equals (id))) {
					albums.Add (_albums.Find (a => a.ID.Equals (id))); 
				} else {
					Album album = new Album (id, name, albumType, images, this);
					albums.Add (album);
					addAlbum (album);
				}
			}
			return albums;
		}

		private List<Track> getTracks (JToken jsonCode) {
			List<Track> tracks = new List<Track> ();
			foreach (JObject jsonTrack in jsonCode) {
				string id = (string)(jsonTrack ["id"]);
				if (!_tracks.Exists (a => a.ID.Equals (id))) {
					string name = (string)(jsonTrack ["name"]);
					int popularity = (int)(jsonTrack ["popularity"]);
					int duration = (int)(jsonTrack ["duration_ms"]);
					bool isExplicit = (bool)jsonTrack ["explicit"];
					int trackNumber = (int)(jsonTrack ["track_number"]);

					List<Artist> artists = getArtists (jsonTrack ["artists"]);

					List<Image> images = getImages (jsonTrack ["album"].ToObject<JObject> ());

					Album album = null;
					string albumID = (string)jsonTrack ["album"] ["id"];
					if (!Albums.Exists (a => a.ID.Equals (albumID))) {
						string albumName = (string)jsonTrack ["album"] ["name"];
						string albumType = (string)jsonTrack ["album"] ["album_type"];
						album = new Album (albumID, albumName, albumType, images, this, artists);
						addAlbum (album);
					} else {
						album = Albums.Find (a => a.ID.Equals (albumID));
					}

					Track track = new Track (id, name, popularity, duration, isExplicit, trackNumber, album, this, artists);

					tracks.Add (track);
					addTrack (track);
				}
			}
			return tracks;
		}

		private List<Image> getImages (JObject imageList) {
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
