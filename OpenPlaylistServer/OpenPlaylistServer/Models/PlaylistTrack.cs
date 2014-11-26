using System.ComponentModel;
using SpotifyDotNet;

namespace OpenPlaylistServer.Models {
    public class PlaylistTrack : Track, INotifyPropertyChanged {
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
                return _tScore + PScore;
            }
        }

        public void UpdatePScore(int scoreDiff) {
            PScore += scoreDiff;
        }

        public void ResetPScore() {
            PScore = 0;
        }

        public int currentDurationStep { get; set; }

        public int PScore { get; private set; }

        public PlaylistTrack(string trackUri)
            : base(trackUri)
        {
            PScore = 0;
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
