using System.Collections.Generic;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IFilterService
    {
        void FilterTracks(IEnumerable<Track> tracks, IEnumerable<Restriction> restrictions);
    }
}
