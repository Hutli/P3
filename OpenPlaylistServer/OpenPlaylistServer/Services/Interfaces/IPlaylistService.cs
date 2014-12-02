using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaylistService
    {
        Track FindTrack(string trackUri);

        void Add(Track track);

        ConcurrentDictify<string,Track> Tracks { get; }
        int CalcTScore(Track track);

        Track NextTrack();
    }
}
