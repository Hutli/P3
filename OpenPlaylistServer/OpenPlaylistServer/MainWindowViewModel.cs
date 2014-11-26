using System.Collections.ObjectModel;

namespace OpenPlaylistServer
{
    class MainWindowViewModel : IMainWindowViewModel
    {
        private IPlaylistService _playlistService;
        private IUserService _userService;
        private IPlaybackService _playbackService;

        public MainWindowViewModel(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService)
        {
            _playlistService = playlistService;
            _userService = userService;
            _playbackService = playbackService;
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
            _playbackService.Stop();
            PlaylistTrack next = _playlistService.NextTrack();
            if(next != null) {
                _playbackService.Play(next);
            }
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
