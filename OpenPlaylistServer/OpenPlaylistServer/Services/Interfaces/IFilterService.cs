using System.Collections.Generic;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IFilterService
    {
        IEnumerable<Track> FilterTracks(IEnumerable<Track> tracks, IEnumerable<Restriction> restrictions);
    }
}
