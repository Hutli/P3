using System;
using System.ComponentModel;
using System.Windows;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Models {
    public class User : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private IPlaybackService _playbackService;
        private float _volume;
        private Track _vote;

        public string Id { get; private set; }

        public float Volume
        {
            get { return _volume; }
            set
            {
                if (!(value >= 0) || !(value <= 1)) return;
                _volume = value;
                OnPropertyChanged("Volume");
            }
        }

        public Track Vote
        {
            get { return _vote; }
            set
            {
                _vote = value;
                
                
                // TODO: Den her linje crasher programmet hvis en anden sang er ved at blive spillet... mystisk
                //OnPropertyChanged("Vote");
                
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
