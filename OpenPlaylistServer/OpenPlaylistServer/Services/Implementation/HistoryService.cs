using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;
using System.Collections.ObjectModel;

namespace OpenPlaylistServer.Services.Implementation {
    public class HistoryService : IHistoryService{
        private readonly ObservableCollection<Track> _hist = new ObservableCollection<Track>();

        public ObservableCollection<Track> Tracks
        {
            get { return _hist; }
        }

        public void Add(Track track)
        {
           _hist.Add(track);        
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
