using SpotifyDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
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
