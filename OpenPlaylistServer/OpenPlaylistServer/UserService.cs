using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
{
    public class UserService : IUserService
    {
        private List<User> _users;

        public UserService()
        {
            _users = new List<User>();
        }

        public IEnumerable<User> Users
        {
            get {
                foreach (var item in _users)
                {
                    yield return item;
                }
            }
        }

        public void Add(User user)
        {
            _users.Add(user);
        }
    }
}
