using System.Collections;
using System.Collections.Generic;
using SpotifyDotNet;
using WebAPI;
using Track = WebAPI.Track;

namespace OpenPlaylistServer
{
    class SearchService : ISearchService
    {
        public IEnumerable<Track> Search(string query)
        {
            return WebAPIMethods.Search(query, 20);
        }
    }
}