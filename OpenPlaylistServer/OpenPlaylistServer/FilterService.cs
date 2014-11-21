using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI;

namespace OpenPlaylistServer
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