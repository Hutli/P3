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
		private List<Artist> _artists = new List<Artist> ();
		private List<Album> _albums = new List<Album> ();
		private List<Track> _tracks = new List<Track> ();

		public List<Artist> Artists{ get { return _artists; } }

		public List<Album> Albums{ get { return _albums; } }

		public List<Track> tracks{ get { return _tracks; } }

		public void addArtist(Artist artist){
			if(_artists.Exists(a => a.ID == artist.ID)){
				throw new Exception (); //TODO Create spotify exception
			} else {
				_artists.Add (artist);
			}
		}

		public void addAlbum(Album album){
			if(_albums.Exists(a => a.ID == album.ID)){
				throw new Exception (); //TODO Create spotify exception
			} else {
				_albums.Add (album);
			}
		}

		public void addTrack(Track track){
			if(_tracks.Exists(a => a.ID == track.ID)){
				throw new Exception (); //TODO Create spotify exception
			} else {
				_tracks.Add (track);
			}
		}

		public readonly string searchString;

		public search (string searchString) : this (searchString, SearchType.ALL) {
		}

		public search (string searchString, SearchType type) {
			switch (type) {
			case SearchType.ALL:
			case SearchType.ARTIST:
				_artists = getArtist (searchString);
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

		private List<Artist> getArtist (string searchString) {
			List<Artist> artists = new List<Artist> ();
			string url = "https://api.spotify.com/v1/search?q=" + searchString + "&type=artist";
			JObject o = get (url);
			foreach (JObject artist in o["artists"]["items"]) {
				string id = Convert.ToString (artist ["id"]);
				string name = Convert.ToString (artist ["name"]);
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
				foreach (JObject image in album["images"]) {
					int height = Convert.ToInt32 (image ["height"]);
					int width = Convert.ToInt32 (image ["width"]);
					string imageUrl = Convert.ToString (image ["url"]);
					images.Add (new Image (height, width, imageUrl));
				}
				albums.Add (new Album (id, name, albumType, images));
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
				foreach (JObject image in track["albums"]["images"]) {
					int height = Convert.ToInt32 (image ["height"]);
					int width = Convert.ToInt32 (image ["width"]);
					string imageUrl = Convert.ToString (image ["url"]);
					images.Add (new Image (height, width, imageUrl));
				}
				string albumId = Convert.ToString (track ["album"] ["id"]);
				string albumName = Convert.ToString (track ["album"] ["name"]);
				string albumType = Convert.ToString (track ["album"] ["album_type"]);

				Album album = new Album (albumId, albumName, albumType, images);
				
				tracks.Add (new Track (id, name, popularity, duration, isExplicit, trackNumber, album));
			}
			return tracks;
		}

		/*private List<Image> getImages(string imageList){
			JObject o = new JObject (imageList);
			List<Image> images = new List<Image> ();
			foreach (JObject image in imageList["images"]) {
				int height = Convert.ToInt32 (image ["height"]);
				int width = Convert.ToInt32 (image ["width"]);
				string url = Convert.ToString (image ["url"]);
				images.Add (new Image (height, width, url));
			}
			return images;
		}*/

		public static JObject get (string url) {
			WebClient client = new WebClient ();
			string content = client.DownloadString (url);
			JObject o = JObject.Parse (content);
			return o;
		}
	}
}

