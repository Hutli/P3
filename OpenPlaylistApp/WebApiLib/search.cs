using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
    public enum SearchType {
        All,
        Artist,
        Album,
        Track

    }

    public class Search {
        private List<Artist> _artists = new List<Artist>();
        private List<Album> _albums = new List<Album>();
        private List<Track> _tracks = new List<Track>();

        public List<Artist> Artists { get { return _artists; } }

        public List<Album> Albums { get { return _albums; } }

        public List<Track> Tracks { get { return _tracks; } }

        public void AddArtist(Artist artist) {
            if(!_artists.Exists(a => a.ID == artist.ID)) //No duplicates
                _artists.Add(artist);
        }

        public void AddAlbum(Album album) {
            if(!_albums.Exists(a => a.ID == album.ID)) //No duplicates
                _albums.Add(album);
        }

        public void AddTrack(Track track) {
            if(!_tracks.Exists(a => a.ID == track.ID)) //No duplicates
                _tracks.Add(track);
        }

        public readonly string SearchString;

        public Search(string searchString) : this(searchString, SearchType.All) { }

        public Search(string searchString, SearchType type) {
            switch(type) {
                case SearchType.All:
                case SearchType.Artist:
                    JObject oArtists = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=artist");
                    GetArtists(oArtists["artists"]["items"]); //TODO Vi kalder en metode og smider returværdien væk?
                    if(type == SearchType.All)
                        goto case SearchType.Album; //TODO FIX C#!
                    break;
                case SearchType.Album:
                    JObject oAlbums = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=album");
                    GetAlbums(oAlbums["albums"]["items"]); //TODO Vi kalder en metode og smider returværdien væk?
                    if(type == SearchType.All)
                        goto case SearchType.Track; //TODO FIX C#!
                    break;
                case SearchType.Track:
                    JObject oTracks = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=track");
                    GetTracks(oTracks["tracks"]["items"]); //TODO Vi kalder en metode og smider returværdien væk?
                    break;
            }
        }

        /// <summary>
        /// Gets list of unique artists from JSON
        /// </summary>
        /// <param name="jsonCode">JSON collection of artists</param>
        /// <returns>List of artists contained in JSON</returns>
        private List<Artist> GetArtists(IEnumerable<JToken> jsonCode) {
            List<Artist> artists = new List<Artist>();
            foreach(JObject jsonArtist in jsonCode) {
                string id = (string)(jsonArtist["id"]);
                string name = (string)(jsonArtist["name"]);
                if(_artists.Exists(a => a.ID.Equals(id))) {
                    artists.Add(_artists.Find(a => a.ID.Equals(id)));
                }
                else {
                    Artist artist = new Artist(id, name, this);
                    artists.Add(artist);
                    AddArtist(artist);
                }
            }
            return artists;
        }

        /// <summary>
        /// Gets list of unique albums from JSON
        /// </summary>
        /// <param name="jsonCode">JSON collection of albums</param>
        /// <returns>List of albums contained in JSON</returns>
        private List<Album> GetAlbums(IEnumerable<JToken> jsonCode) { 
            List<Album> albums = new List<Album>();
            foreach(JObject jsonAlbum in jsonCode) {
                string id = (string)(jsonAlbum["id"]);
                string name = (string)(jsonAlbum["name"]);
                string albumType = (string)(jsonAlbum["album_type"]);
                IEnumerable<Image> images = GetImages(jsonAlbum);
                if(_albums.Exists(a => a.ID.Equals(id))) {
                    albums.Add(_albums.Find(a => a.ID.Equals(id)));
                }
                else {
                    Album album = new Album(id, name, albumType, images, this);
                    albums.Add(album);
                    AddAlbum(album);
                }
            }
            return albums;
        }

        /// <summary>
        /// Gets list of unique tracks from JSON
        /// </summary>
        /// <param name="jsonCode">JSON collection of tracks</param>
        /// <returns>List of tracks contained in JSON</returns>
        private List<Track> GetTracks(IEnumerable<JToken> jsonCode) {
            List<Track> tracks = new List<Track>();
            foreach(JObject jsonTrack in jsonCode) {
                string id = (string)(jsonTrack["id"]);
                if (_tracks.Exists(a => a.ID.Equals(id))) continue;
                string name = (string)(jsonTrack["name"]);
                int popularity = (int)(jsonTrack["popularity"]);
                int duration = (int)(jsonTrack["duration_ms"]);
                bool isExplicit = (bool)jsonTrack["explicit"];
                int trackNumber = (int)(jsonTrack["track_number"]);

                List<Artist> artists = GetArtists(jsonTrack["artists"]);

                IEnumerable<Image> images = GetImages(jsonTrack["album"].ToObject<JObject>());

                Album album;
                string albumId = (string)jsonTrack["album"]["id"];
                if(!Albums.Exists(a => a.ID.Equals(albumId))) {
                    string albumName = (string)jsonTrack["album"]["name"];
                    string albumType = (string)jsonTrack["album"]["album_type"];
                    album = new Album(albumId, albumName, albumType, images, this, artists);
                    AddAlbum(album);
                }
                else {
                    album = Albums.Find(a => a.ID.Equals(albumId));
                }

                Track track = new Track(id, name, popularity, duration, isExplicit, trackNumber, album, this, artists);

                tracks.Add(track);
                AddTrack(track);
            }
            return tracks;
        }

        /// <summary>
        /// Gets list of images from JSON
        /// </summary>
        /// <param name="imageList">JSON collection of images</param>
        /// <returns>List of images in JSON</returns>
        private IEnumerable<Image> GetImages(IDictionary<string, JToken> imageList) {
            List<Image> images = new List<Image>();
            foreach(JObject image in imageList["images"]) {
                int height = (int)(image["height"]);
                int width = (int)(image["width"]);
                string imageUrl = (string)(image["url"]);
                images.Add(new Image(height, width, imageUrl));
            }
            return images;
        }

        /// <summary>
        /// Gets JObject from link to Spotify
        /// </summary>
        /// <param name="url">Link to Spotify</param>
        /// <returns>JObject retrieved from Spotify</returns>
        public static JObject GetJobject(string url) {
            string str = Request.Get(url);
            JObject o = JObject.Parse(str);
            return o;
        }
    }
}
