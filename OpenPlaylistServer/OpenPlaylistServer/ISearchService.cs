using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;
using Track = WebAPI.Track;

namespace OpenPlaylistServer
{
    public interface ISearchService
    {
        IEnumerable<Track> Search(string query);
    }
}
