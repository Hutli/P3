using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaylistService
    {
        PlaylistTrack FindTrack(string trackUri);

        void AddByURI(string trackId);
        void Add(PlaylistTrack track);

        ConcurrentBagify<PlaylistTrack> Tracks { get; }

        PlaylistTrack NextTrack();
    }
}
