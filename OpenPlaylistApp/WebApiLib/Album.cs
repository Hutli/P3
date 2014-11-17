using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebAPILib {
    public class Album : SpotifyObject {
        private List<Image> _images = new List<Image>();
        private List<Artist> _artists = new List<Artist>();
        private List<Track> _tracks = new List<Track>();

        public bool TracksCached { get; private set; }

        public bool ArtistsCached { get; private set; }

        public string AlbumType { get; private set; }

        public List<Image> Images { get { return new List<Image>(_images); } }

        public List<Artist> Artists {
            get {
				if (!ArtistsCached) //If artists aren't loaded, load artists from spotify
					Cache ();
                return new List<Artist>(_artists);
            }
        }

		public List<Track> Tracks { 
			get {
                if(!TracksCached) //If artists aren't loaded, load artists from spotify
					Cache ();
				return new List<Track>(_tracks);
			} 
		}

        public string Href { get { return "https://api.spotify.com/v1/albums/" + ID; } }

        /// <summary>
        /// Loads artists and tracks from Spotify into album's artists and tracks
        /// Only runs once (if _artistsCached or _tracksCached is false)
        /// </summary>
        private void Cache()
        {
            JObject o = Search.GetJobject(Href);
            if (!ArtistsCached)
            { //Load artists
                List<Artist> artists = SearchResult.GetArtists(o["artists"]);
                foreach (Artist a in artists)
                {
                    a.AddAlbum(this);
                }
                _artists = artists;
                ArtistsCached = true;
            }
            if (!TracksCached)
            { //Load Tracks
                List<Track> tracks = new List<Track>();
                foreach (JObject jsonTrack in o["tracks"]["items"])
                {
                    string id = (string)(jsonTrack["id"]);
                    if (SearchResult.Tracks.Exists(a => id.Equals(a.ID)))
                        _tracks.Add(SearchResult.Tracks.Find(a => id.Equals(a.ID)));
                    else
                    {
                        string name = (string)(jsonTrack["name"]);
                        int popularity = (int)(jsonTrack["popularity"]);
                        int duration = (int)(jsonTrack["duration_ms"]);
                        bool isExplicit = (bool)jsonTrack["explicit"];
                        int trackNumber = (int)(jsonTrack["track_number"]);
                        Track tmpTrack = new Track(id, name, popularity, duration, isExplicit, trackNumber, SearchResult, this); //TODO Spotify don't want to tell ud popularity
                        SearchResult.AddTrack(tmpTrack);
                        _tracks.Add(tmpTrack);
                    }
                }
                TracksCached = true;
            }
        }

        public override string URI { get { return "spotify:album:" + ID; } }

        public Album(string id, string name, string albumtype, IEnumerable<Image> images, Search searchResult, List<Artist> artists)
            : this(id, name, albumtype, images, searchResult)
        {
            AddArtists(artists);
            foreach (Artist a in artists)
            {
                a.AddAlbum(this);
            }
        }

		public Album(string id, string name, string albumtype, IEnumerable<Image> images, Search searchResult) : base(id, name, searchResult) {
            AlbumType = albumtype;
            _images = new List<Image>(images);
        }

        /// <summary>
        /// Adds artists to album
        /// </summary>
        /// <param name="artists"> Artist to be added</param>
		public void AddArtists(List<Artist> artists){
            if (_artists.Count != 0) return;
            _artists = artists;
            foreach (Artist a in _artists) {
                a.AddAlbum (this);
            }
            ArtistsCached = true;
		}

        /// <summary>
        /// Adds track to album
        /// </summary>
        /// <param name="track">Track to be added</param>
        public void AddTrack(Track track)
        {
            if (!_tracks.Exists(a => track.ID.Equals(a.ID)))
                _tracks.Add(track);
        }

        public override string ToString()
        {
            string returnString = string.Format("{0} by ", Name);
            foreach (Artist a in Artists)
            {
                if (Artists.FindLast(x => true) == a)
                    returnString += string.Format(a.Name);
                else
                    returnString += string.Format("{0}, ", a.Name);
            }
            return returnString;
        }
    }
}