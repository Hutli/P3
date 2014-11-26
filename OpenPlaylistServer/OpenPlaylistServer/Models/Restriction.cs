using System;
using WebAPI;

namespace OpenPlaylistServer.Models
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
