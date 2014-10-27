using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OpenPlaylistServer
{
    public interface IUserService
    {
        ReadOnlyObservableCollection<User> Users { get; }

        void Add(User user);
    }
}
