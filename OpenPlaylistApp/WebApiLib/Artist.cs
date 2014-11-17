using System.Collections.Generic;

namespace WebAPILib {
	public class Artist: SpotifyObject {
		private List<string> _genres = new List<string> ();
		private List<Album> _albums = new List<Album> ();

	    public Artist (string id, string name, Search searchResult) : base (id, name, searchResult) {

        public List<string> Genres { get { return new List<string>(_genres); } }

        public bool AlbumsCached { get { return _albumsCached; } }

<<<<<<< 89a92838e03891fa2117d454d5144112f7ffc202
	    public int Popularity { get; private set; }

	    public override string URI{ get { return "spotify:artist:" + ID; } }
        public string Href { get { return "https://api.spotify.com/v1/artists/" + ID; } }

        public List<Album> Albums
        {
            get
            {
                JObject o = Search.getJobject(Href);
                if (!_albumsCached)
                { //Load albums
                    List<Album> albums = SearchResult.GetAlbums(o["albums"]);
                    foreach (Album a in albums)
                    {
                        a.AddArtists(new List<Artist> { this });
                    }
                    _albums = albums;
                    _albumsCached = true;
                }
                return new List<Album>(_albums);
            }
        }

        public override string URI { get { return "spotify:artist:" + ID; } }

        /// <summary>
        /// Adds album to artist
        /// </summary>
        /// <param name="album">Album to be added</param>
        public void AddAlbum(Album album)
        {
            if (!_albums.Exists(a => a.ID.Equals(album.ID))) //No duplicates
                _albums.Add(album);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}