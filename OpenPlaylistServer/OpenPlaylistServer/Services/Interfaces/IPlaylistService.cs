using System.Collections.ObjectModel;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaylistService
    {
        PlaylistTrack FindTrack(string trackUri);

        void AddByURI(string trackId);
        void Add(PlaylistTrack track);

        void Remove(PlaylistTrack track);
        void RemoveByTitle(string name);

        ReadOnlyObservableCollection<PlaylistTrack> Tracks { get; }

        PlaylistTrack NextTrack();
    }
}
