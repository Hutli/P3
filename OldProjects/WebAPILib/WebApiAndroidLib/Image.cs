using System;

namespace WebAPILib {
	public class Image {
		private int _height;
		private int _width;
		private string _url;

		public Image (int height, int width, string url) {
			_height = height;
			_width = width;
			_url = url;
		}

		public int Height{ get { return _height; } }

		public int Width{ get { return _width; } }

		public string URL{ get { return _url; } }

	}
}

