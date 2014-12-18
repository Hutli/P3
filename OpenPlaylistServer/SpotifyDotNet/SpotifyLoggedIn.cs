using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using libspotifydotnet;

namespace SpotifyDotNet {
    /// <summary>
    ///     Interaction with spotify services. Must be logged in through Spotify class.
    /// </summary>
    public class SpotifyLoggedIn {
        private static SpotifyLoggedIn _instance;
        private bool _isPlaying;
        private readonly IntPtr _searchComplete;
        private readonly IntPtr _sessionPtr;
        private readonly object _sync;
        private SpotifyLoggedIn() {}

        internal SpotifyLoggedIn(ref IntPtr sessionPtr, object sync, ref IntPtr searchComplete) {
            _instance = this;
            _searchComplete = searchComplete;
            _sync = sync;
            _sessionPtr = sessionPtr;
        }

        /// <summary>
        ///     Get an instance of SpotifyLoggedIn. Throws exception if not logged in via Spotify class.
        /// </summary>
        public static SpotifyLoggedIn Instance {
            get {
                if(_instance == null)
                    throw new NullReferenceException("Not logged into Spotify");
                return _instance;
            }
        }

        /// <summary>
        ///     Starts a search. Will return results in the SearchComplete event on Spotify class.
        /// </summary>
        /// <param name="query">The query to search for</param>
        public void Search(string query) {
            var queryPointer = Marshal.StringToHGlobalAnsi(query);

            lock(_sync) {
                var searchPtr = libspotify.sp_search_create(_sessionPtr,
                                                            queryPointer,
                                                            0,
                                                            10,
                                                            0,
                                                            10,
                                                            0,
                                                            10,
                                                            0,
                                                            10,
                                                            sp_search_type.SP_SEARCH_STANDARD,
                                                            _searchComplete,
                                                            IntPtr.Zero); //TODO vi bruger aldrig searchPtr?
            }
        }

        /// <summary>
        ///     Start receiving audio data on the given track. Data is delivered through the MusicDelivery event.
        /// </summary>
        /// <param name="track">The track to receive audio data on</param>
        public void Play(Track track) {
            if(_isPlaying) {
                Console.WriteLine("Ind i isplaying");
                Stop();
            }

            var err = track.Load(_sessionPtr);

            if(err != libspotify.sp_error.OK)
                Console.WriteLine("error loading trackplayer load: " + err);

            Console.WriteLine("Track should now be loaded successfully");

            var playErr = libspotify.sp_session_player_play(_sessionPtr, true);
            _isPlaying = true;
            Console.WriteLine("player status: " + playErr);
        }

        /// <summary>
        ///     Stop receiving audio data.
        /// </summary>
        public void Stop() {
            try {
                if(_isPlaying) {
                    Console.WriteLine("Trying to unload player");
                    libspotify.sp_session_player_unload(_sessionPtr);
                    Console.WriteLine("Successfully unloaded player");
                } else
                    Console.WriteLine("There was nothing to stop");
            } catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }

            _isPlaying = false;
        }

        /// <summary>
        ///     Get a track from a spotify Uri. The Uri must be a valid spotify track Uri.
        /// </summary>
        /// <param name="Uri">Valid Spotify track Uri</param>
        /// <returns></returns>
        public Task<Track> TrackFromLink(String Uri) {
            try {
                //lock (_sync)
                //{
                var t = Task.Run(() => {
                    var linkPtr = Marshal.StringToHGlobalAnsi(Uri);
                    var spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);
                    if(spLinkPtr == IntPtr.Zero) {
                        Console.WriteLine("Uri was not a track Uri");
                        throw new ArgumentException("Uri was not a track Uri");
                    }
                    var linkType = libspotify.sp_link_type(spLinkPtr);

                    if(linkType == libspotify.sp_linktype.SP_LINKTYPE_TRACK) {
                        var trackLinkPtr = libspotify.sp_link_as_track(spLinkPtr);
                        if(trackLinkPtr == IntPtr.Zero)
                            Console.WriteLine("sp_link_as_track error");

                        return new Track(trackLinkPtr);
                    }

                    // in case an error happened
                    Console.WriteLine("Error: Returning null track in TrackFromLink");
                    return null;
                });

                return t;
                //}
            } catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}