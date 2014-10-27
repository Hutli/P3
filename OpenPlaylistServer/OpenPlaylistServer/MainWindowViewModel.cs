using SpotifyDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OpenPlaylistServer
{
    class MainWindowViewModel : IMainWindowViewModel
    {
        private IPlaylistService _playlistService;
        

        public MainWindowViewModel(IPlaylistService playlistService)
        {
            this._playlistService = playlistService;
        }

        public ReadOnlyObservableCollection<PlaylistTrack> Tracks
        {
            get
            {
                return _playlistService.Tracks;
            }
        }


        public void TrackEnded()
        {
            PlaylistTrack next = _playlistService.NextTrack();
            //history.Add(next);
            //SpotifyLoggedIn.Instance.Play(next.Track);
            //UpdateUI();
        }


        public PlaylistTrack NextTrack()
        {
            return _playlistService.NextTrack();
        }


        public void PlayButonClicked()
        {
            //history.Add(next);
            SpotifyLoggedIn.Instance.Play(NextTrack());
        }

    }
}
