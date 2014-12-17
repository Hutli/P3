using System.Collections.ObjectModel;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaylistService
    {
        ObservableCollection<Track> Tracks { get; }
        Track FindTrack(string trackUri);
        void Add(Track track);
        int CalcTScore(Track track);
        Track NextTrack();
        void Remove(Track track);
    }
}