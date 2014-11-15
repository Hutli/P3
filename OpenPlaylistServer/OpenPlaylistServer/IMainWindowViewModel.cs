using System.Collections.ObjectModel;

namespace OpenPlaylistServer
{
    public interface IMainWindowViewModel
    {
        ReadOnlyObservableCollection<PlaylistTrack> Tracks { get; }

        void TrackEnded();

        void PlayButtonClicked();

        void StopButtonClicked();
    }
}
