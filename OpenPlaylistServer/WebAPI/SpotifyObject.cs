namespace WebAPI {
	public abstract class SpotifyObject {
		protected string _id;
		protected string _name;

	    protected SpotifyObject (string id, string name) {
			_id = id;
			_name = name;
		}

        public string ID { get { return _id; } }

        public string Name { get { return _name; } }

        public virtual string URI { get { return ""; } } //TODO generate URI

        public override bool Equals(object obj)
        {
            return ((SpotifyObject)obj).ID.Equals(ID);
        }
    }
}