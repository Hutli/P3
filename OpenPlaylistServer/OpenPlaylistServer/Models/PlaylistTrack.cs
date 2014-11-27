using System.ComponentModel;
//using SpotifyDotNet;
using WebAPI;

namespace OpenPlaylistServer.Models {
    public class PlaylistTrack : Track, INotifyPropertyChanged {
        private int _tScore;
        private int _pScore;

        public int TScore
        {
            get
            {
                return _tScore;
            }
            set
            {
                _tScore = value;
                OnPropertyChanged("TScore");
            }
        }

        public int TotalScore
        {
            get
            {
                return _tScore + PScore;
            }
        }

        public void UpdatePScore(int scoreDiff) {
            PScore += scoreDiff;
        }

        public void ResetPScore() {
            PScore = 0;
        }

        public int PScore
        {
            get { return _pScore; }
            private set
            {
                _pScore = value;
                OnPropertyChanged("PScore");
            }
        }

        public PlaylistTrack(string trackUri)
        {
            Track track = WebAPIMethods.GetTrack(trackUri);
            _name = track.Name;
            ISRC = track.ISRC;
            Duration = track.Duration;
            Id = track.Id;
            IsExplicit = track.IsExplicit;
            TrackNumber = track.TrackNumber;
            PreviewURL = track.PreviewURL;
            Album = track.Album;
            IsFiltered = false;
        }

        public override string ToString()
        {
            return Name;
        }

		public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string pName) {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        } 
    }
}
