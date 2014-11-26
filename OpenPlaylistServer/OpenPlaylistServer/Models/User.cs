using System.ComponentModel;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Models {
    public class User : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private IPlaybackService _playbackService;
        private float _volume;
        private PlaylistTrack _vote;

        public string Id { get; private set; }

        public float Volume
        {
            get { return _volume; }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }

        public PlaylistTrack Vote
        {
            get { return _vote; }
            set
            {
                _vote = value;
                OnPropertyChanged("Vote");
            }
        }

        public User(string id, IPlaybackService playbackService) {
            _playbackService = playbackService;
            Volume = _playbackService.GetCurrentVolume();
            PropertyChanged += _playbackService.SetCurrentVolume;
            Id = id;
        }


        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        } 
    }
}
