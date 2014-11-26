using System.Collections.Concurrent;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Services.Implementation
{
    public class UserService : IUserService
    {
        readonly ConcurrentBagify<User> _users;

        public UserService()
        {
            _users = new ConcurrentBagify<User>();
            //_roUsers = new ReadOnlyObservableCollection<User>(_users);
        }

        public ConcurrentBagify<User> Users
        {
            get {
                return _users;
            }
        }

        public void Add(User user)
        {
            _users.Add(user);
        }
    }
}
