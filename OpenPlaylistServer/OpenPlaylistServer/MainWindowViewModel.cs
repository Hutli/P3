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
        private IUserService _userService;
        private IPlaybackService _playbackService;

        public MainWindowViewModel(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService)
        {
            this._playlistService = playlistService;
            this._userService = userService;
            this._playbackService = playbackService;
        }

        public ReadOnlyObservableCollection<PlaylistTrack> Tracks
        {
            get
            {
                return _playlistService.Tracks;
            }
        }

        public ReadOnlyObservableCollection<User> Users
        {
            get
            {
                return _userService.Users;
            }
        }

        public void TrackEnded()
        {
            PlaylistTrack next = _playlistService.NextTrack();
        }

        public void PlayButtonClicked()
        {
            var nextTrack = _playlistService.NextTrack();
            if (nextTrack != null)
            {
                _playbackService.Play(nextTrack);
            }
        }

        public void StopButtonClicked()
        {
            _playbackService.Stop();
        }
    }
}
