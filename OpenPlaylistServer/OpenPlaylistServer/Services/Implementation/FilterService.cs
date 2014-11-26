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
            var tempTracks = tracks;
            foreach (var restriction in restrictions)
            {
                tempTracks = tempTracks.Where(restriction.Predicate);
            }

            return tempTracks;
        }
    }
}