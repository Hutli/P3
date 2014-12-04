using System.Collections.ObjectModel;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IMainWindowViewModel
    {
        ObservableCollection<Track> Tracks { get; }

        void TrackEnded();

        void PlayButtonClicked();

        void StopButtonClicked();
    }
}
