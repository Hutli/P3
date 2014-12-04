using System.Collections.Generic;
using OpenPlaylistServer.Models;
using WebAPI;
using System.Collections.ObjectModel;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IRestrictionService
    {

        ObservableCollection<Restriction> Restrictions { get; }
        void RestrictTracks(IEnumerable<Track> tracks);

        void AddRestriction(Restriction restriction);
    }
}
