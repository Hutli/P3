using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IUserService
    {
        ConcurrentBagify<User> Users { get; }

        void Add(User user);
    }
}
