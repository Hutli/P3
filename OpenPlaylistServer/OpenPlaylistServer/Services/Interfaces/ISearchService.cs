using System.Collections.Generic;
using System.Threading.Tasks;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<Track>> Search(string query, int offset = 0);
    }
}
