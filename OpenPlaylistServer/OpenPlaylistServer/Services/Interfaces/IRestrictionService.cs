using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces {
    public interface IRestrictionService {
        ObservableCollection<Restriction> Restrictions {
            get;
        }

        void RestrictTracks(IEnumerable<Track> tracks);
        void AddRestriction(Restriction restriction);
        void RemoveRestriction(Restriction restriction);
    }
}