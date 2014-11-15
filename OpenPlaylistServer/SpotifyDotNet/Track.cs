using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Availability = libspotifydotnet.libspotify.sp_availability;
using SpError = libspotifydotnet.libspotify.sp_error;

namespace SpotifyDotNet
{
    public class Track : IDisposable
    {
        private IntPtr _trackPtr;

        /// <summary>
        /// The name of the track
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Has the track loaded all its metadata?
        /// </summary>
        public Boolean IsLoaded { get { return libspotify.sp_track_is_loaded(_trackPtr); } }

        /// <summary>
        /// Duration of the track in milliseconds
        /// </summary>
        public int Duration { get { return libspotify.sp_track_duration(_trackPtr); } }
        
        /// <summary>
        /// The spotify track URI. Will only get correct string if track object was created with a spotify URI.
        /// </summary>
        public string Uri { get; private set; }

        internal Track(IntPtr trackPtr)
        {
            Init(trackPtr);
        }

        private void Init(IntPtr trackPtr)
        {
            this._trackPtr = trackPtr;

            // wait until track is loaded including metadata
            while (libspotify.sp_track_is_loaded(_trackPtr) == false)
            {
                // do not destroy cpu
                Task.Delay(1);
            }

            // name
            IntPtr trackNamePtr = libspotify.sp_track_name(_trackPtr);
            Name = Marshal.PtrToStringAnsi(trackNamePtr);
        }

        public Track(string trackUri)
        {
            _trackPtr = SpotifyLoggedIn.Instance.TrackUriToIntPtr(trackUri);
            Uri = trackUri;
            Init(_trackPtr);
        }

        ~Track()
        {
            Dispose();
        }

        /// <summary>
        /// Tell spotify to load this track for playback.
        /// </summary>
        /// <param name="sessionPtr">The sessionPtr of the spotify session</param>
        /// <returns>Status of operation</returns>
        internal SpError Load(IntPtr sessionPtr)
        {
            return libspotify.sp_session_player_load(sessionPtr, _trackPtr);
        }

        internal Availability GetAvailability(IntPtr sessionPtr)
        {
            return libspotify.sp_track_get_availability(sessionPtr, _trackPtr);
        }

        public void Dispose()
        {
            libspotify.sp_track_release(_trackPtr);

            System.GC.SuppressFinalize(this);
        }
    }
}
