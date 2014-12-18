using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAPI {
    [JsonObject(MemberSerialization.OptOut)]
    public class Album : SpotifyObject {
        private readonly List<Artist> _artists = new List<Artist>();
        private readonly List<Image> _images = new List<Image>();

        public Album(string id, string name, string albumtype, IEnumerable<Image> images, List<Artist> artists)
            : base(id, name) {
            AlbumType = albumtype;
            _images = new List<Image>(images);
            _artists = new List<Artist>(artists);
        }

        public string ArtistsToString {
            get {
                var returnString = string.Empty;
                foreach(var a in Artists) {
                    if(Equals(Artists.FindLast(x => true), a))
                        returnString += string.Format(a.Name);
                    else
                        returnString += string.Format("{0}, ", a.Name);
                }
                return returnString;
            }
        }

        public bool TracksCached {
            get;
            private set;
        }

        public bool ArtistsCached {
            get;
            private set;
        }

        private string AlbumType {
            get;
            set;
        }

        public List<Image> Images {
            get {return new List<Image>(_images);}
        }

        [JsonProperty]
        public List<Artist> Artists {
            get {return new List<Artist>(_artists);}
        }

        public string Href {
            get {return "https://api.spotify.com/v1/albums/" + Id;}
        }

        public override string Uri {
            get {return "spotify:album:" + Id;}
        }

        public override string ToString() {
            return Name;
        }
    }
}