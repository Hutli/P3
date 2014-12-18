using System;
using System.Collections.Generic;
using libspotifydotnet;

namespace SpotifyDotNet {
    public class SearchResult : IDisposable {
        private readonly IntPtr _searchPtr;

        internal SearchResult(IntPtr searchPtr) {
            Tracks = new List<Track>();
            _searchPtr = searchPtr;
            var numTracks = libspotify.sp_search_num_tracks(searchPtr);

            for(var i = 0; i < numTracks; i++) {
                var trackPtr = libspotify.sp_search_track(searchPtr, i);
                var track = new Track(trackPtr);
                Tracks.Add(track);
            }
        }

        private List<Track> Tracks {
            get;
            set;
        }

        public void Dispose() {
            libspotify.sp_search_release(_searchPtr);

            GC.SuppressFinalize(this);
        }

        ~SearchResult() {
            Dispose();
        }
    }
}