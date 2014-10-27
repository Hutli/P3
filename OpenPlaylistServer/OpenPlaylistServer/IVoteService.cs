using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
{
    public interface IVoteService
    {
        void Vote(string userId, string trackUri);
    }
}
