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
        private IntPtr trackPtr;

        public String Name { get; private set; }
        public Boolean IsLoaded { get { return libspotify.sp_track_is_loaded(trackPtr); } }
        public int Duration { get { return libspotify.sp_track_duration(trackPtr); } }

        

        public Track(IntPtr trackPtr)
        {
            this.trackPtr = trackPtr;

            // name
            IntPtr trackNamePtr = libspotify.sp_track_name(trackPtr);
            Name = Marshal.PtrToStringAnsi(trackNamePtr);
        }

        public SpError Load(IntPtr sessionPtr)
        {
            return libspotify.sp_session_player_load(sessionPtr, trackPtr);
        }

        public Availability GetAvailability(IntPtr sessionPtr)
        {
            return libspotify.sp_track_get_availability(sessionPtr, trackPtr);
        }

        ~Track() {
            Dispose();
        }

        public void Dispose()
        {
            libspotify.sp_track_release(trackPtr);

            System.GC.SuppressFinalize(this);
        }
    }
}
