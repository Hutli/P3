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

        public String Name { get; private set; }
        public Boolean IsLoaded { get { return libspotify.sp_track_is_loaded(_trackPtr); } }
        public int Duration { get { return libspotify.sp_track_duration(_trackPtr); } }

        internal Track(IntPtr trackPtr)
        {
            this._trackPtr = trackPtr;

            // wait until track is loaded including metadata
            while (libspotify.sp_track_is_loaded(_trackPtr) == false)
            {
                Task.Delay(2);
            }

            // name
            IntPtr trackNamePtr = libspotify.sp_track_name(_trackPtr);
            Name = Marshal.PtrToStringAnsi(trackNamePtr);
        }

        ~Track()
        {
            Dispose();
        }

        public SpError Load(IntPtr sessionPtr)
        {
            return libspotify.sp_session_player_load(sessionPtr, _trackPtr);
        }

        public Availability GetAvailability(IntPtr sessionPtr)
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
