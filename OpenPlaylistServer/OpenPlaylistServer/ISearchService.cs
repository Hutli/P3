using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;
using WebAPILib;

namespace OpenPlaylistServer
{
    public interface ISearchService
    {
        Search Search(string query);
    }
}
