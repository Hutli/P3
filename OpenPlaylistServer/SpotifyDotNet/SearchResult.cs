using libspotifydotnet;
using System;
using System.Collections.Generic;

namespace SpotifyDotNet
{
    public class SearchResult : IDisposable
    {
        private IntPtr _searchPtr;

        public List<Track> Tracks { get; private set; }

        internal SearchResult(IntPtr searchPtr)
        {
            Tracks = new List<Track>();
            _searchPtr = searchPtr;
            int numTracks = libspotify.sp_search_num_tracks(searchPtr);
         
            for (int i = 0; i < numTracks; i++)
            {
                IntPtr trackPtr = libspotify.sp_search_track(searchPtr, i);
                Track track = new Track(trackPtr);
                Tracks.Add(track);
            }
        }

        ~SearchResult()
        {
            Dispose();
        }

        public void Dispose()
        {
            libspotify.sp_search_release(_searchPtr);

            GC.SuppressFinalize(this);
        }
    }
}
