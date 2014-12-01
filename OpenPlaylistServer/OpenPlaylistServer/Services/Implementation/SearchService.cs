using System.Collections.Generic;
using System.Threading.Tasks;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Implementation
{
    class SearchService : ISearchService
    {
        public async Task<IEnumerable<Track>> Search(string query, int offset = 0)
        {
            return await WebAPIMethods.Search(query, 20, offset);
        }
    }
}