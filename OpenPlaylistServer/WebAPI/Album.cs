using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Album : SpotifyObject
    {
        private List<Image> _images = new List<Image>();
        private List<Artist> _artists = new List<Artist>();

        public bool TracksCached { get; private set; }

        public bool ArtistsCached { get; private set; }

        public string AlbumType { get; private set; }

        public List<Image> Images { get { return new List<Image>(_images); } }

        [JsonProperty]
        public List<Artist> Artists { get { return new List<Artist>(_artists); } }

        public string Href { get { return "https://api.spotify.com/v1/albums/" + ID; } }

        public override string URI { get { return "spotify:album:" + ID; } }

        public Album(string id, string name, string albumtype, IEnumerable<Image> images, List<Artist> artists)
            : base(id, name)
        {
            AlbumType = albumtype;
            _images = new List<Image>(images);
            _artists = new List<Artist>(artists);
        }

        public override string ToString()
        {
            string returnString = string.Format("{0} by ", Name);
            foreach (Artist a in Artists)
            {
                if (Equals(Artists.FindLast(x => true), a))
                    returnString += string.Format(a.Name);
                else
                    returnString += string.Format("{0}, ", a.Name);
            }
            return returnString;
        }
    }
}