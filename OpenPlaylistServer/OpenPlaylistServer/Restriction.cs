using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace OpenPlaylistServer
{
    public class Restriction
    {
        private readonly Func<Track,bool> _predicate;

        public Restriction(Func<Track, bool> predicate)
        {
            _predicate = predicate;
        }


        public Func<Track, bool> Predicate
        {
            get { return _predicate; }
        }
    }
}
