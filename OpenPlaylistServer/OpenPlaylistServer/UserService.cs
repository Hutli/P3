using System.Collections.ObjectModel;

namespace OpenPlaylistServer
{
    public class UserService : IUserService
    {
        readonly ObservableCollection<User> _users;
        readonly ReadOnlyObservableCollection<User> _roUsers;

        public UserService()
        {
            _users = new ObservableCollection<User>();
            _roUsers = new ReadOnlyObservableCollection<User>(_users);
        }

        public ReadOnlyObservableCollection<User> Users
        {
            get {
                return _roUsers;
            }
        }

        public void Add(User user)
        {
            _users.Add(user);
        }
    }
}
