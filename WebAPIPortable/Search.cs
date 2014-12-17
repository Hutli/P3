using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebAPI
{
    public static class WebAPIMethods
    {
        public static async Task<IEnumerable<Track>> Search(string searchString, int limit, int offset = 0)
        {
            var jsonTracks = await GetJobject("https://api.spotify.com/v1/search?limit=" + limit + "&offset=" + offset + "&q=" + searchString + "&type=track&market=DK");
            return GetTracks(jsonTracks["tracks"]["items"]);
        }

        private static List<Track> GetTracks(IEnumerable<JToken> jsonCode)
        {
            var tracks = new List<Track>();
            var albums = new List<Album>();
            var artists = new List<Artist>();

            foreach(var jsonTrack in jsonCode)
            {
                var id = (string)(jsonTrack["id"]);
                var isrc = (string)(jsonTrack["external_ids"]["isrc"]);

                if(tracks.Exists(a => a.Equals(id, isrc)))
                    continue;
                var name = (string)(jsonTrack["name"]);
                if(name.EndsWith("- Live") || name.EndsWith("(Live)") || name.EndsWith("- Live Version"))
                    continue;
                var duration = (int)(jsonTrack["duration_ms"]);
                var isExplicit = (bool)jsonTrack["explicit"];
                var trackNumber = (int)(jsonTrack["track_number"]);
                var previewUrl = (string)(jsonTrack["preview_Url"]);

                var tmpArtists = GetArtists(jsonTrack["artists"], artists);

                var tmpAlbum = GetAlbum(jsonTrack["album"], tmpArtists, albums);
                tracks.Add(new Track(id, name, duration, isExplicit, trackNumber, isrc, previewUrl, tmpAlbum));
            }
            return tracks;
        }

        public static Track GetTrack(string uri)
        {
            var artists = new List<Artist>();
            var uriId = uri.Substring(14);

            var jsonTrack = GetJobject("https://api.spotify.com/v1/tracks/" + uriId).Result;
            // removes first 14 first char of spotify:track:1zHlj4dQ8ZAtrayhuDDmkY
            if(jsonTrack == null)
                return null;

            var id = (string)(jsonTrack["id"]);
            var isrc = (string)(jsonTrack["external_ids"]["isrc"]);
            var name = (string)(jsonTrack["name"]);
            var duration = (int)(jsonTrack["duration_ms"]);
            var isExplicit = (bool)jsonTrack["explicit"];
            var trackNumber = (int)(jsonTrack["track_number"]);
            var previewUrl = (string)(jsonTrack["preview_Url"]);

            var tmpArtists = GetArtists(jsonTrack["artists"], artists);

            var tmpAlbum = GetAlbum(jsonTrack["album"], tmpArtists, new List<Album>());

            return new Track(id, name, duration, isExplicit, trackNumber, isrc, previewUrl, tmpAlbum);
        }

        private static Album GetAlbum(JToken jsonCode, List<Artist> artists, List<Album> inputAlbums)
        {
            Album album;
            var id = (string)(jsonCode["id"]);
            if(inputAlbums.Exists(a => a.Id.Equals(id)))
                album = inputAlbums.Find(a => a.Id.Equals(id));
            else
            {
                var name = (string)(jsonCode["name"]);
                var albumType = (string)(jsonCode["album_type"]);
                var images = GetImages(jsonCode.ToObject<JObject>());
                album = new Album(id, name, albumType, images, artists);
                inputAlbums.Add(album);
            }
            return album;
        }

        private static List<Artist> GetArtists(IEnumerable<JToken> jsonCode, List<Artist> inputArtists)
        {
            var artists = new List<Artist>();
            foreach(var jsonArtist in jsonCode)
            {

                var id = (string)(jsonArtist["id"]);
                if(inputArtists.Exists(a => a.Id.Equals(id)))
                    artists.Add(inputArtists.Find(a => a.Id.Equals(id)));
                else
                {
                    var name = (string)(jsonArtist["name"]);
                    var artist = new Artist(id, name);
                    inputArtists.Add(artist);
                    artists.Add(artist);
                }
            }
            return artists;
        }

        private static IEnumerable<Image> GetImages(IDictionary<string, JToken> imageList)
        {
            var images = new List<Image>();
            foreach(var jToken in imageList["images"])
            {
                var image = (JObject)jToken;
                var height = (int)(image["height"]);
                var width = (int)(image["width"]);
                var imageUrl = (string)(image["url"]);
                images.Add(new Image(height, width, imageUrl));
            }
            return images;
        }

        private static async Task<JObject> GetJobject(string url)
        {
            var str = await Request.Get(url);
            if(str == null)
                return null;
            var o = JObject.Parse(str);
            return o;
        }
    }
}