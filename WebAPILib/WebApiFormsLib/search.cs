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
				_artists = getArtists (searchString);
				if (type == SearchType.ALL)
					goto case SearchType.ALBUM; //TODO FIX C#!
				break;
			case SearchType.ALBUM:
				_albums = getAlbums (searchString);
				if (type == SearchType.ALL)
					goto case SearchType.TRACK; //TODO FIX C#!
				break;
			case SearchType.TRACK:
				_tracks = getTracks (searchString);
				break;
			}
		}

		private List<Artist> getArtists (string searchString) {
			List<Artist> artists = new List<Artist> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=artist";
			JObject o = getJobject (url);
			foreach (JObject jsonArtist in o["artists"]["items"]) {
				string id = (string)(jsonArtist ["id"]);
				string name = (string)(jsonArtist ["name"]);
				Artist artist = new Artist (id, name, this);
				artists.Add (artist);
			}
			return artists;
		}

		private List<Album> getAlbums (string searchString) {
			List<Album> albums = new List<Album> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=album";
			JObject o = getJobject (url);
			foreach (JObject jsonAlbum in o["albums"]["items"]) {
				string id = (string)(jsonAlbum ["id"]);
				string name = (string)(jsonAlbum ["name"]);
				string albumType = (string)(jsonAlbum ["album_type"]);
				List<Image> images = getImages (jsonAlbum);
				Album album = new Album (id, name, albumType, images, this);
				albums.Add (album);
			}
			return albums;
		}

		private List<Track> getTracks (string searchString) {
			List<Track> tracks = new List<Track> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=track";
			JObject o = getJobject (url);
			foreach (JObject jsonTrack in o["tracks"]["items"]) {
				string id = (string)(jsonTrack ["id"]);
				string name = (string)(jsonTrack ["name"]);
				int popularity = (int)(jsonTrack ["popularity"]);
				int duration = (int)(jsonTrack ["duration_ms"]);
				bool isExplicit = (bool)jsonTrack ["explicit"];
				int trackNumber = (int)(jsonTrack ["track_number"]);

				List<Artist> artists = new List<Artist> ();

				foreach (JObject artist in jsonTrack["artists"])
					artists.Add (new Artist ((string)(artist ["id"]), (string)(artist ["name"]), this));

				List<Image> images = getImages (jsonTrack ["album"].ToObject<JObject> ());

				string albumId = (string)(jsonTrack ["album"] ["id"]);
				string albumName = (string)(jsonTrack ["album"] ["name"]);
				string albumType = (string)(jsonTrack ["album"] ["album_type"]);

				Album album = new Album (albumId, albumName, albumType, images, this);

				Track track = new Track (id, name, popularity, duration, isExplicit, trackNumber, album, this, artists);
				foreach (Artist a in artists)
					addArtist (a);
				addAlbum (album);
				tracks.Add (track);
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
