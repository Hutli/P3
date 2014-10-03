using System;
using System.Collections.Generic;
using WebAPILib;
using System.Drawing;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
	public enum SearchType {
		ALL,
		ARTIST,
		ALBUM,
		TRACK}

	;

	public class search {
		public List<Artist> artists = new List<Artist> ();
		public List<SpotifyObject> results = new List<SpotifyObject> ();
		public readonly string searchString;



		public search (string searchString) : this (searchString, SearchType.ALL) {
		}

		public search (string searchString, SearchType type) {
			switch (type) {
			case SearchType.ALL:
			case SearchType.ARTIST:
				results.AddRange (getArtist (searchString));
				if (type == SearchType.ALL)
					goto case SearchType.ALBUM; //TODO FIX C#!
				break;
			case SearchType.ALBUM:
				results.AddRange (getAlbums (searchString));
				if (type == SearchType.ALL)
					goto case SearchType.TRACK; //TODO FIX C#!
				break;
			case SearchType.TRACK:
				results.AddRange (getTracks (searchString));
				break;
			}
		}

		private List<Artist> getArtist (string searchString) {
			List<Artist> artists = new List<Artist> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=artist";
			JObject o = get (url);
			foreach (JObject artist in o["artists"]["items"]) {
				string id = Convert.ToString (artist ["id"]);
				string name = Convert.ToString (artist ["name"]);
				// TODO If SearchResults contains an artist where the ID is equal, that artist is added to the results
				artists.Add (new Artist (id, name));
			}
			return artists;
		}

		private List<Album> getAlbums (string searchString) {
			List<Album> albums = new List<Album> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=album";
			JObject o = get (url);
			foreach (JObject album in o["albums"]["items"]) {
				string id = Convert.ToString (album ["id"]);
				string name = Convert.ToString (album ["name"]);
				string albumType = Convert.ToString (album ["album_type"]);

				List<Image> images = new List<Image> ();
				foreach (JObject image in album["images"])
					images.Add (new Image(Convert.ToInt32 (image ["height"]), Convert.ToInt32(image["width"]), Convert.ToString(image["url"])));

				// TODO If SearchResults contains an album within its artist where the ID is equal, that album is added to the results
				albums.Add(new Album(id, name, albumType, images));
			}
			return albums;
		}

		private List<Track> getTracks (string searchString) {
			List<Track> tracks = new List<Track> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=track";
			JObject o = get (url);
			foreach (JObject track in o["tracks"]["items"]) {
				string id = Convert.ToString (track ["id"]);
				string name = Convert.ToString (track ["name"]);
				int popularity = Convert.ToInt32 (track ["popularity"]);
				int duration = Convert.ToInt32 (track ["duration_ms"]);
				bool isExplicit = Convert.ToBoolean (track ["explicit"]);
				int trackNumber = Convert.ToInt32 (track ["track_number"]);

				List<Artist> artists = new List<Artist> ();
				foreach (JObject artist in track["artists"])
					artists.Add (new Artist (Convert.ToString (artist ["id"]), Convert.ToString (artist ["name"])));

				List<Image> images = new List<Image> ();
				foreach (JObject image in track ["album"]["images"])
					images.Add (new Image(Convert.ToInt32 (image ["height"]), Convert.ToInt32(image["width"]), Convert.ToString(image["url"])));


				string albumId = Convert.ToString(track["album"] ["id"]);
				string albumName = Convert.ToString (track ["album"] ["name"]);
				string albumType = Convert.ToString (track ["album"] ["album_type"]);

				Album album = new Album (albumId, albumName, albumType, images, artists);
				
				tracks.Add (new Track (id, name, popularity, duration, isExplicit, trackNumber, album));
			}
			return tracks;
		}

		private static JObject get (string url) {
			WebClient client = new WebClient ();
			string content = client.DownloadString (url);
			JObject o = JObject.Parse (content);
			return o;
		}
	}
}

