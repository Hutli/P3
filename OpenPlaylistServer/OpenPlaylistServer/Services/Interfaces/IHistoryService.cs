using System.Collections.Generic;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces {
    public interface IHistoryService
    {
        void Add(Track track);
        void AddRange(IEnumerable<Track> tracks);
        Track GetLastTrack();
        IEnumerable<Track> GetLastNTracks(int n);
    }
}
