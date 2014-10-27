using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPlaylistServer
{
    public interface IUserService
    {
        IEnumerable<User> Users { get; }

        void Add(User user);
    }
}
