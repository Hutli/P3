using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation {
    public class HistoryService : IHistoryService{
        private static readonly List<Track> _hist = new List<Track>();

        public void Add(Track track)
        {
            _hist.Add(track);
        }

        public void AddRange(IEnumerable<Track> tracks)
        {
            _hist.AddRange(tracks);
        }

        public Track GetLastTrack()
        {
            return _hist.LastOrDefault();
        }
        public IEnumerable<Track> GetLastNTracks(int n)
        {
            var tmp = new List<Track>(_hist);
            tmp.Reverse();
            return tmp.Count < n ? tmp : tmp.Take(n);
        }
    }
}
