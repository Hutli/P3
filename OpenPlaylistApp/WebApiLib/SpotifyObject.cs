using Newtonsoft.Json;

namespace WebAPILib {
    [JsonObject(MemberSerialization.OptIn)]
	public abstract class SpotifyObject {
		protected string _id;
		protected string _name;

	    protected SpotifyObject (string id, string name, Search searchResult) {
			_id = id;
			_name = name;
			SearchResult = searchResult;
		}

        public Search SearchResult { get; set; }

        public string ID { get { return _id; } }

        [JsonProperty]
        public string Name { get { return _name; } }

        public virtual string URI { get { return ""; } } //TODO generate URI

        public override bool Equals(object obj)
        {
            SpotifyObject spotifyObject = obj as SpotifyObject;
            if (spotifyObject == null)
            {
                return false;
            }
            return spotifyObject.ID.Equals(ID);
        }
    }
}