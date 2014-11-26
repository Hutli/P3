using System;
using Newtonsoft.Json;

namespace WebAPI {
    [JsonObject(MemberSerialization.OptOut)]
	public abstract class SpotifyObject {
		protected string _id;
		protected string _name;

	    protected SpotifyObject (string id, string name) {
			_id = id;
			_name = name;
		}

        protected SpotifyObject()
        {
            
        }

        public string Id
        {
            get
            {
                return _id;
            }
            protected set { _id = value; }
        }

        [JsonProperty]
        public string Name { get { return _name; } }

        public virtual string URI { get { return ""; } } //TODO generate URI

        public override bool Equals(object obj)
        {
            SpotifyObject spotifyObject = obj as SpotifyObject;

            return spotifyObject != null && spotifyObject.Id.Equals(Id);
        }
    }
}