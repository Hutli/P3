using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IUserService
    {
        ConcurrentDictify<string, User> Users { get; }

        void Add(User user);
    }
}
