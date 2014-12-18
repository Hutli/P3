using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using libspotifydotnet;
using Availability = libspotifydotnet.libspotify.sp_availability;
using SpError = libspotifydotnet.libspotify.sp_error;

namespace SpotifyDotNet {
    public class Track : IDisposable {
        private IntPtr _trackPtr;

        internal Track(IntPtr trackPtr) {
            Init(trackPtr);
        }

        /// <summary>
        ///     The name of the track
        /// </summary>
        public String Name {
            get;
            private set;
        }

        /// <summary>
        ///     Has the track loaded all its metadata?
        /// </summary>
        public Boolean IsLoaded {
            get {return libspotify.sp_track_is_loaded(_trackPtr);}
        }

        /// <summary>
        ///     Duration of the track in milliseconds
        /// </summary>
        public int Duration {
            get {return libspotify.sp_track_duration(_trackPtr);}
        }

        /// <summary>
        ///     The spotify track Uri. Will only get correct string if track object was created with a spotify Uri.
        /// </summary>
        public string Uri {
            get;
            private set;
        }

        public void Dispose() {
            if(_trackPtr != IntPtr.Zero) {
                //libspotify.sp_track_release(_trackPtr);
            }

            GC.SuppressFinalize(this);
        }

        private void Init(IntPtr trackPtr) {
            _trackPtr = trackPtr;

            // wait until track is loaded including metadata
            while(libspotify.sp_track_is_loaded(_trackPtr) == false) {
                // do not destroy cpu
                Task.Delay(1);
            }

            // name
            var trackNamePtr = libspotify.sp_track_name(_trackPtr);
            Name = Marshal.PtrToStringAnsi(trackNamePtr);
        }

        ~Track() {
            Dispose();
        }

        /// <summary>
        ///     Tell spotify to load this track for playback.
        /// </summary>
        /// <param name="sessionPtr">The sessionPtr of the spotify session</param>
        /// <returns>Status of operation</returns>
        internal SpError Load(IntPtr sessionPtr) {
            return libspotify.sp_session_player_load(sessionPtr, _trackPtr);
        }

        internal Availability GetAvailability(IntPtr sessionPtr) {
            return libspotify.sp_track_get_availability(sessionPtr, _trackPtr);
        }
    }
}