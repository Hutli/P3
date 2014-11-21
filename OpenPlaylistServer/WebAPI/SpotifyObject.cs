using System;
using Newtonsoft.Json;

namespace WebAPI {
    [JsonObject(MemberSerialization.OptIn)]
	public abstract class SpotifyObject {
		protected string _id;
		protected string _name;

	    protected SpotifyObject (string id, string name) {
			_id = id;
			_name = name;
		}

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