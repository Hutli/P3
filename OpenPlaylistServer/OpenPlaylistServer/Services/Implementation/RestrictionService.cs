using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation {
    public class RestrictionService : IRestrictionService {
        private readonly ObservableCollection<Restriction> _restrictions = new ObservableCollection<Restriction>();

        public ObservableCollection<Restriction> Restrictions {
            get {return _restrictions;}
        }

        public void RestrictTracks(IEnumerable<Track> tracks) {
            var whitelistFound = false;
            foreach(var t in tracks) {
                var isBlacklisted = false;
                var isWhitelisted = false;
                foreach(var r in _restrictions) {
                    if(!r.IsActive)
                        continue;
                    if(r.RestrictionType == RestrictionType.BlackList && r.Predicate(t)) {
                        isBlacklisted = true;
                        break;
                    }

                    if(r.RestrictionType == RestrictionType.WhiteList) {
                        whitelistFound = true;
                        if(!r.Predicate(t))
                            isWhitelisted = true;
                    }
                }

                t.IsFiltered = isBlacklisted || (whitelistFound && !isWhitelisted);
            }
        }

        public void AddRestriction(Restriction restriction) {
            _restrictions.Add(restriction);
        }

        public void RemoveRestriction(Restriction restriction) {
            _restrictions.Remove(restriction);
        }
    }
}