using System;

namespace WebAPILib
{
	public abstract class SpotifyObject
	{
		private string _id;
		private string _name;

		public SpotifyObject (int id, string name)
		{
			_id = id;
			_name = name;
		}

		public string ID { get{return _id;} }

		public string Name { get{return _name;} }

		public string URI { get; }
	}
}

