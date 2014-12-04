using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using System.Collections.ObjectModel;

namespace OpenPlaylistServer.Services.Implementation
{
    public class UserService : IUserService
    {
        readonly ObservableCollection<User> _users;

        public UserService()
        {
            _users = new ObservableCollection<User>();
            //_roUsers = new ReadOnlyObservableCollection<User>(_users);
        }

        public ObservableCollection<User> Users
        {
            get {
                return _users;
            }
        }

        public void Add(User user)
        {
            _users.Add(user);
            user.CheckedIn = true;
        }
    }
}
