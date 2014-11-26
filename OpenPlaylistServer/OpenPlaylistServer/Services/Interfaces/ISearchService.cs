using System.Collections.Generic;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<Track> Search(string query);
    }
}
