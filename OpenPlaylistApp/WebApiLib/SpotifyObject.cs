namespace WebAPILib {
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

		public string Name { get { return _name; } }

		public virtual string URI { get { return ""; } } //TODO generate URI
	}
}