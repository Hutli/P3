using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IMainWindowViewModel
    {
        ConcurrentDictify<string, Track> Tracks { get; }

        void TrackEnded();

        void PlayButtonClicked();

        void StopButtonClicked();
    }
}
