using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
    public enum SearchType {
        All,
        Artist,
        Album,
        Track
    }

    public class Search
    {
        private List<Artist> _artists = new List<Artist>();
        private List<Album> _albums = new List<Album>();
        private List<Track> _tracks = new List<Track>();

        public bool LockSearch;

        public List<Artist> Artists { get { return _artists; } }

        public List<Album> Albums { get { return _albums; } }

        public List<Track> Tracks { get { return _tracks; } }

        public void AddArtist(Artist artist)
        {
            if (!_artists.Exists(a => a.Equals(artist))) //No duplicates
                _artists.Add(artist);
        }

        public void AddAlbum(Album album)
        {
            if (!_albums.Exists(a => a.Equals(album))) //No duplicates
                _albums.Add(album);
        }

        public void AddTrack(Track track)
        {
            if (!_tracks.Exists(a => a.Equals(track))) //No duplicates
                _tracks.Add(track);
        }

        public readonly string SearchString;

        public Search(string searchString) : this(searchString, SearchType.All) { }

        public Search(string searchString, SearchType type)
        {
            switch (type)
            {
                case SearchType.All:
                    JObject json = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=artist,album,track&market=DK");
                    foreach (Artist a in GetArtists(json["artists"]["items"]))
                        AddArtist(a);
                    foreach (Album a in GetAlbums(json["albums"]["items"]))
                        AddAlbum(a);
                    foreach (Track t in GetTracks(json["tracks"]["items"]))
                        AddTrack(t);
                    break;
                case SearchType.Artist:
                    JObject jsonArtist = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=artist&market=DK");
                    foreach (Artist a in GetArtists(jsonArtist["artists"]["items"]))
                        AddArtist(a);
                    break;
                case SearchType.Album:
                    JObject jsonAlbums = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=album&market=DK");
                    foreach (Album a in GetAlbums(jsonAlbums["albums"]["items"]))
                        AddAlbum(a);
                    break;
                case SearchType.Track:
                    JObject jsonTracks = GetJobject("https://api.spotify.com/v1/search?limit=10&q=" + searchString + "&type=track&market=DK");
                    foreach (Track t in GetTracks(jsonTracks["tracks"]["items"]))
                        AddTrack(t);
                    break;
            }
        }

        /// <summary>
        /// Gets list of unique artists from JSON
        /// </summary>
        /// <param name="jsonCode">JSON collection of artists</param>
        /// <returns>List of artists contained in JSON</returns>
        public List<Artist> GetArtists(IEnumerable<JToken> jsonCode) {
            List<Artist> artists = new List<Artist>();
            foreach(var jToken in jsonCode) {
                var jsonArtist = (JObject) jToken;
                string id = (string)(jsonArtist["id"]);
                if (_artists.Exists(a => a.ID.Equals(id)))
                    artists.Add(_artists.Find(a => a.ID.Equals(id)));
                else
                {
                    string name = (string)(jsonArtist["name"]);
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
        
        public List<Album> GetAlbums(IEnumerable<JToken> jsonCode, List<Artist> inputArtists)
        {
            List<Album> albums = new List<Album>();
            foreach (var jToken in jsonCode)
            {
                var jsonAlbum = (JObject) jToken;
                string id = (string)(jsonAlbum["id"]);
                if (_albums.Exists(a => a.ID.Equals(id)))
                    albums.Add(_albums.Find(a => a.ID.Equals(id)));
                else
                {
                    string name = (string)(jsonAlbum["name"]);
                    string albumType = (string)(jsonAlbum["album_type"]);
                    IEnumerable<Image> images = GetImages(jsonAlbum.ToObject<JObject>());
                    Album album = new Album(id, name, albumType, images, this, inputArtists);
                    albums.Add(album);
                    AddAlbum(album);
                }
            }
            return albums;
        }

        public List<Album> GetAlbums(IEnumerable<JToken> jsonCode)
        {
            return GetAlbums(jsonCode, new List<Artist>());
        }

        /// <summary>
        /// Gets list of unique tracks from JSON
        /// </summary>
        /// <param name="jsonCode">JSON collection of tracks</param>
        /// <returns>List of tracks contained in JSON</returns>

        public List<Track> GetTracks(IEnumerable<JToken> jsonCode) {
            List<Track> tracks = new List<Track>();
            foreach(var jToken in jsonCode) {
                var jsonTrack = (JObject) jToken;
                string id = (string)(jsonTrack["id"]);
                Track track;
                if (_tracks.Exists(a => a.ID.Equals(id)))
                    track = _tracks.Find(a => a.ID.Equals(id));
                else
                {
                    string name = (string)(jsonTrack["name"]);
                    int popularity = (int)(jsonTrack["popularity"]);
                    int duration = (int)(jsonTrack["duration_ms"]);
                    bool isExplicit = (bool)jsonTrack["explicit"];
                    int trackNumber = (int)(jsonTrack["track_number"]);

                    List<Artist> artists = GetArtists(jsonTrack["artists"]);

                    Album album; //= GetAlbums(jsonTrack["album"], artists)[0];
                    
                    string albumID = (string)(jsonTrack["album"]["id"]);
                    if (_albums.Exists(a => a.ID.Equals(id)))
                        album = _albums.Find(a => a.ID.Equals(id));
                    else
                    {
                        string albumName = (string)(jsonTrack["album"]["name"]);
                        string albumType = (string)(jsonTrack["album"]["album_type"]);
                        IEnumerable<Image> images = GetImages(jsonTrack["album"].ToObject<JObject>());
                        album = new Album(id, name, albumType, images, this, artists);
                        AddAlbum(album);
                    }

                    track = new Track(id, name, popularity, duration, isExplicit, trackNumber, this, album, artists);
                }
                tracks.Add(track);
                AddTrack(track);
            }
            return tracks;
        }

        /*public void EvaluateAll()
        {
            JObject artists = getJobject("https://api.spotify.com/v1/artists?ids=" + GetAllIds(_artists.Select<Artist, SpotifyObject>(a => (SpotifyObject)a).ToList()));
            JObject albums = getJobject("https://api.spotify.com/v1/artists?ids=" + GetAllIds(_albums.Select<Album, SpotifyObject>(a => (SpotifyObject)a).ToList()));
            JObject tracks = getJobject("https://api.spotify.com/v1/artists?ids=" + GetAllIds(_tracks.Select<Track, SpotifyObject>(t => (SpotifyObject)t).ToList()));
            //tracks.
        }

        private string GetAllIds(List<SpotifyObject> inputList){
            string ids = "";
            foreach (SpotifyObject s in inputList)
            {
                if (!(s == inputList.FindLast(x => true)))
                {
                    ids += string.Format("{0},", s.ID);
                }
                else
                {
                    ids += string.Format("{0}", s.ID);
                }
            }
            return ids;
        }*/

        /// <summary>
        /// Gets list of images from JSON
        /// </summary>
        /// <param name="imageList">JSON collection of images</param>
        /// <returns>List of images in JSON</returns>
        private IEnumerable<Image> GetImages(IDictionary<string, JToken> imageList) {
            List<Image> images = new List<Image>();
            foreach(var jToken in imageList["images"]) {
                var image = (JObject) jToken;
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
