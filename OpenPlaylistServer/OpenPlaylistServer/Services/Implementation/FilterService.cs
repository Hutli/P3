using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation
{
    class FilterService : IFilterService
    {
        public void FilterTracks(IEnumerable<Track> tracks, IEnumerable<Restriction> restrictions)
        {
            //return restrictions.Aggregate(tracks, (current, restriction) => current.Where(restriction.Predicate));
            foreach (Track t in tracks)
            {
                foreach (Restriction r in restrictions)
                {
                    if (!r.Predicate(t))
                    {
                        t.IsFiltered = true;
                        break;
                    }
                }
            }
        }
    }
}