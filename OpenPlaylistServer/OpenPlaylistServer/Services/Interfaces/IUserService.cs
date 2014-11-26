using System.Collections.ObjectModel;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IUserService
    {
        ReadOnlyObservableCollection<User> Users { get; }

        void Add(User user);
    }
}
