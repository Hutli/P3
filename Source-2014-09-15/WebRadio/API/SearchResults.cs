using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRadio.API
{
    public class SearchResults
    {
        private IntPtr searchPtr;
        private int _numTracks;

        public List<Track> Tracks { get; private set; }

        public SearchResults(IntPtr searchPtr)
        {
            Tracks = new List<Track>();
            this.searchPtr = searchPtr;
            _numTracks = libspotify.sp_search_num_tracks(searchPtr);
         
            for (int i = 0; i < _numTracks; i++)
            {
                IntPtr trackPtr = libspotify.sp_search_track(searchPtr, i);
                Track track = new Track(trackPtr);
                Tracks.Add(track);
            }
        }

    }
}
