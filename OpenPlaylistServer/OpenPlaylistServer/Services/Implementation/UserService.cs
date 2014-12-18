using System.Collections.ObjectModel;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ObservableCollection<User> _users;

        public UserService()
        {
            _users = new ObservableCollection<User>();
            //_roUsers = new ReadOnlyObservableCollection<User>(_users);
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
        }

        public void Add(User user)
        {
            _users.Add(user);
            user.CheckedIn = true;
        }
    }
}