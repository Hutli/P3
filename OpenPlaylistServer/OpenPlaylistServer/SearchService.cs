using SpotifyDotNet;
using WebAPILib;

namespace OpenPlaylistServer
{
    class SearchService : ISearchService
    {
        public Search Search(string query)
        {
            var searchResult = new Search(query);
            return searchResult;
        }
    }
}