namespace OpenPlaylistApp
{
    public class Venue
    {
        private string _name;
        private string _detail;
        private string _ip;
        private string _iconUrl;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public string IconUrl
        {
            get { return _iconUrl; }
            set { _iconUrl = value; }
        }
        public Venue(string name, string detail, string ip, string iconUrl)
        {
            _name = name;
            _detail = detail;
            _ip = ip;
            _iconUrl = iconUrl;
        }
    }
}
