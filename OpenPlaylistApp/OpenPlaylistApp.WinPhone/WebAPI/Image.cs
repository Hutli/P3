using Newtonsoft.Json;

namespace WebAPI {
    [JsonObject(MemberSerialization.OptOut)]
	public class Image {
	    public Image (int height, int width, string url) {
			Height = height;
			Width = width;
			URL = url;
		}

	    public int Height { get; private set; }

	    public int Width { get; private set; }

	    public string URL { get; private set; }
	}
}

