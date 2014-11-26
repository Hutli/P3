using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation
{
    class FilterService : IFilterService
    {
        public IEnumerable<Track> FilterTracks(IEnumerable<Track> tracks, IEnumerable<Restriction> restrictions)
        {
            return restrictions.Aggregate(tracks, (current, restriction) => current.Where(restriction.Predicate));
        }
    }
}