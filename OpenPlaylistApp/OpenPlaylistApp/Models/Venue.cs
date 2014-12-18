namespace OpenPlaylistApp.Models {
    public class Venue {
        public Venue(string name, string detail, string ip, string iconUrl) {
            Name = name;
            Detail = detail;
            IP = ip;
            IconUrl = iconUrl;
        }

        public string Name {
            get;
            set;
        }

        public string Detail {
            get;
            set;
        }

        public string IP {
            get;
            set;
        }

        public string IconUrl {
            get;
            set;
        }
    }
}