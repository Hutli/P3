using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IHistoryService
    {
        ObservableCollection<Track> Tracks { get; }
        void Add(Track track);
        Track GetLastTrack();
        IEnumerable<Track> GetLastNTracks(int n);
    }
}