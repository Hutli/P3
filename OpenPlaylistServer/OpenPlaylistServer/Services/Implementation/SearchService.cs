using System.Collections.Generic;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Implementation
{
    class SearchService : ISearchService
    {
        public IEnumerable<Track> Search(string query)
        {
            return WebAPIMethods.Search(query, 20);
        }
    }
}