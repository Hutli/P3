using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebRadio.API
{
    public class Track
    {
        public IntPtr trackPtr;

        public String Name { get; private set; }

        public Track(IntPtr trackPtr)
        {
            this.trackPtr = trackPtr;

            // name
            IntPtr trackNamePtr = libspotify.sp_track_name(trackPtr);
            Name = Marshal.PtrToStringAnsi(trackNamePtr);
        }
    }
}
