using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace OpenPlaylistServer
{
    public interface IFilterService
    {
        IEnumerable<Track> FilterTracks(IEnumerable<Track> tracks, IEnumerable<Restriction> restrictions);
    }
}
