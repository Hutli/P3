using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyDotNet
{
    public class SearchResults : IDisposable
    {
        private IntPtr _searchPtr;
        private int _numTracks;

        public List<Track> Tracks { get; private set; }

        public SearchResults(IntPtr searchPtr)
        {
            Tracks = new List<Track>();
            this._searchPtr = searchPtr;
            _numTracks = libspotify.sp_search_num_tracks(searchPtr);
         
            for (int i = 0; i < _numTracks; i++)
            {
                IntPtr trackPtr = libspotify.sp_search_track(searchPtr, i);
                Track track = new Track(trackPtr);
                Tracks.Add(track);
            }
        }

        ~SearchResults()
        {
            Dispose();
        }

        public void Dispose()
        {
            libspotify.sp_search_release(_searchPtr);

            System.GC.SuppressFinalize(this);
        }
    }
}
