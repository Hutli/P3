using Newtonsoft.Json;

namespace WebAPI {
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class SpotifyObject {
        private string _id;
        private readonly string _name;

        protected SpotifyObject(string id, string name) {
            _id = id;
            _name = name;
        }

        protected SpotifyObject() {}

        public string Id {
            get {return _id;}
            set {_id = value;}
        }

        [JsonProperty]
        public string Name {
            get {return _name;}
        }

        public virtual string Uri {
            get {return "";}
        }

        public override bool Equals(object obj) {
            var spotifyObject = obj as SpotifyObject;

            return spotifyObject != null && spotifyObject.Id.Equals(Id);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}