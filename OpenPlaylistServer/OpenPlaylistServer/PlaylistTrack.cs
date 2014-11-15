using SpotifyDotNet;
using System.ComponentModel;

namespace OpenPlaylistServer {
    public class PlaylistTrack : Track, INotifyPropertyChanged {
        private int _pScore;
        private int _tScore;

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
                return _tScore + _pScore;
            }
        }

        public void UpdatePScore(int scoreDiff) {
            _pScore += scoreDiff;
        }

        public void ResetPScore() {
            _pScore = 0;
        }

        public int PScore
        {
            get
            {
                return _pScore;
            }
        }

        public PlaylistTrack(string trackUri)
            : base(trackUri)
        {
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
