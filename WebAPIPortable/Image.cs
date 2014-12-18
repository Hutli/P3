using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Image
    {
        public Image(int height, int width, string url)
        {
            Height = height;
            Width = width;
            Url = url;
        }

        private int Height { get; set; }
        private int Width { get; set; }
        public string Url { get; private set; }
    }
}