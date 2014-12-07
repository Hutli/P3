using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using System.Collections.ObjectModel;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IMainWindowViewModel
    {
        ObservableCollection<Track> Tracks { get; }

        ObservableCollection<Track> History { get; }

        ObservableCollection<Restriction> Restrictions { get; }

        void AddRestriction(Restriction restriction);

        void TrackEnded();

        void PlayButtonClicked();

        void StopButtonClicked();

        void RemoveTrack_Click(Track track);
        void RemoveRestriction(Restriction restriction);
    }
}
