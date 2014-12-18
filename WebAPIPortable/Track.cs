using System.ComponentModel;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Track : SpotifyObject, INotifyPropertyChanged
    {
        private int _pScore;
        private int _tScore;

        [JsonConstructor]
        public Track(string id, string name, int duration, bool isExplicit, int trackNumber, string isrc, string previewUrl, Album album) : base(id, name)
        {
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
            Isrc = isrc;
            PreviewUrl = previewUrl;
            Album = album;
            //IsFiltered = false;
        }

        public int TScore
        {
            get { return _tScore; }
            set
            {
                _tScore = value;
                OnPropertyChanged("TScore");
            }
        }

        public int PScore
        {
            get { return _pScore; }
            set
            {
                _pScore = value;
                OnPropertyChanged("PScore");
            }
        }

        public string Isrc { get; set; }
        public bool IsFiltered { get; set; }
        private string PreviewUrl { get; set; }
        public int Duration { get; private set; }
        public int CurrentDurationStep { get; set; }
        public bool IsSelected { get; set; }
        private bool IsExplicit { get; set; }
        private int TrackNumber { get; set; }

        public int TotalScore
        {
            get { return _pScore + _tScore; }
        }

        [JsonProperty]
        public Album Album { get; private set; }

        public string ToStringProperty
        {
            get { return ToString(); }
        }

        public override string Uri
        {
            get { return "spotify:track:" + Id; }
        }

        public int Rank { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string pName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }

        public override string ToString() { return string.Format("{0} - {1}", Name, Album.ArtistsToString); }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var track = obj as Track;
                if (track != null)
                    return track.Id.Equals(Id) || track.Isrc.Equals(Isrc);
            }
            return false;
        }

        public bool Equals(string id, string isrc) { return (Id == id || Isrc == isrc); }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}