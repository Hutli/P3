using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IMainWindowViewModel
    {
        ConcurrentBagify<PlaylistTrack> Tracks { get; }

        void TrackEnded();

        void PlayButtonClicked();

        void StopButtonClicked();
    }
}
