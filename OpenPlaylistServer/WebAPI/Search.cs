using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebAPI
{
    public static class WebAPIMethods
    {
        public static IEnumerable<Track> Search(string searchString, int limit)
        {
            JObject jsonTracks = GetJobject("https://api.spotify.com/v1/search?limit=" + limit + "&q=" + searchString + "&type=track&market=DK");
            return GetTracks(jsonTracks["tracks"]["items"]);
        }

        private static List<Track> GetTracks(IEnumerable<JToken> jsonCode)
        {
            List<Track> tracks = new List<Track>();
            List<Album> albums = new List<Album>();
            List<Artist> artists = new List<Artist>();

            foreach (var jsonTrack in jsonCode)
            {
                string id = (string)(jsonTrack["id"]);
                string isrc = (string)(jsonTrack["external_ids"]["isrc"]);

                if (!tracks.Exists(a => a.Id == id || a.ISRC == isrc))
                {
                    string name = (string) (jsonTrack["name"]);
                    int duration = (int) (jsonTrack["duration_ms"]);
                    bool isExplicit = (bool) jsonTrack["explicit"];
                    int trackNumber = (int) (jsonTrack["track_number"]);
                    string previewURL = (string) (jsonTrack["preview_url"]);



                    List<Artist> tmpArtists = GetArtists(jsonTrack["artists"], artists);

                    Album tmpAlbum = GetAlbum(jsonTrack["album"], artists, albums);

                    tracks.Add(new Track(id, name, duration, isExplicit, trackNumber, isrc, previewURL, tmpAlbum));
                }
            }
            return tracks;
        }

        public static Track GetTrack(string uri)
        {
            List<Artist> artists = new List<Artist>();
            var uriId = uri.Substring(14);

            JObject jsonTrack = GetJobject("https://api.spotify.com/v1/tracks/" + uriId); // removes first 14 first char of spotify:track:1zHlj4dQ8ZAtrayhuDDmkY

            string id = (string)(jsonTrack["id"]);
            string isrc = (string)(jsonTrack["external_ids"]["isrc"]);
            string name = (string)(jsonTrack["name"]);
            int duration = (int)(jsonTrack["duration_ms"]);
            bool isExplicit = (bool)jsonTrack["explicit"];
            int trackNumber = (int)(jsonTrack["track_number"]);
            string previewURL = (string)(jsonTrack["preview_url"]);

            List<Artist> tmpArtists = GetArtists(jsonTrack["artists"], artists);

            Album tmpAlbum = GetAlbum(jsonTrack["album"], artists, new List<Album>());

            return new Track(id, name, duration, isExplicit, trackNumber, isrc, previewURL, tmpAlbum);
        }

        private static Album GetAlbum(JToken jsonCode, List<Artist> artists, List<Album> inputAlbums)
        {
            Album album;
            string id = (string)(jsonCode["id"]);
            if (inputAlbums.Exists(a => a.Id.Equals(id)))
            {
                album = inputAlbums.Find(a => a.Id.Equals(id));
            }
            else
            {
                string name = (string)(jsonCode["name"]);
                string albumType = (string)(jsonCode["album_type"]);
                IEnumerable<Image> images = GetImages(jsonCode.ToObject<JObject>());
                album = new Album(id, name, albumType, images, artists);
                inputAlbums.Add(album);
            }
            return album;
        }

        private static List<Artist> GetArtists(IEnumerable<JToken> jsonCode, List<Artist> inputArtists)
        {
            List<Artist> artists = new List<Artist>();
            foreach (var jsonArtist in jsonCode)
            {
                //var sJsonArtist = GetJobject((string)jsonArtist["href"]);

                string id = (string)(jsonArtist["id"]);
                if (inputArtists.Exists(a => a.Id.Equals(id)))
                {
                    artists.Add(inputArtists.Find(a => a.Id.Equals(id)));
                }
                else
                {
                    string name = (string)(jsonArtist["name"]);
                    //List<string> genres = sJsonArtist["genres"].Select(s => (string) s).ToList();
                    Artist artist = new Artist(id, name);
                    inputArtists.Add(artist);
                    artists.Add(artist);
                }
            }
            return artists;
        }

        private static IEnumerable<Image> GetImages(IDictionary<string, JToken> imageList)
        {
            List<Image> images = new List<Image>();
            foreach (var jToken in imageList["images"])
            {
                var image = (JObject)jToken;
                int height = (int)(image["height"]);
                int width = (int)(image["width"]);
                string imageUrl = (string)(image["url"]);
                images.Add(new Image(height, width, imageUrl));
            }
            return images;
        }

        private static JObject GetJobject(string url)
        {
            string str = Request.Get(url);
            JObject o = JObject.Parse(str);
            return o;
        }
    }
}
