using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;
using System.Collections.ObjectModel;

namespace OpenPlaylistServer.Services.Implementation
{
    class RestrictionService : IRestrictionService
    {

        private ObservableCollection<Restriction> _restrictions = new ObservableCollection<Restriction>();

        public ObservableCollection<Restriction> Restrictions { get { return _restrictions; } }

        public void RestrictTracks(IEnumerable<Track> tracks)
        {
            foreach (Track t in tracks)
            {
                foreach (Restriction r in _restrictions)
                {
                    if (!r.Predicate(t))
                    {
                        t.IsFiltered = true;
                        break;
                    }
                }
            }
        }

        public void AddRestriction(Restriction restriction){
            _restrictions.Add(restriction);
        }
    }
}