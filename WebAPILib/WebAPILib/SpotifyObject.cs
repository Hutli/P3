using System;

namespace WebAPILib {
	public abstract class SpotifyObject {
		protected string _id;
		protected string _name;

		public SpotifyObject (string id, string name) {
			_id = id;
			_name = name;
		}

		public search SearchResult { get; set; }

		public string ID { get { return _id; } }

		public string Name { get { return _name; } }

		public virtual string URI { get { return ""; } }
		//TODO generate URI
	}
}

