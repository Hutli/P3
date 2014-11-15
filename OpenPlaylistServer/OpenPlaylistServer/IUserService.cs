using System.Collections.ObjectModel;

namespace OpenPlaylistServer
{
    public interface IUserService
    {
        ReadOnlyObservableCollection<User> Users { get; }

        void Add(User user);
    }
}
