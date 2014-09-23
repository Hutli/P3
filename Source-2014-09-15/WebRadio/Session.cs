using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libspotifydotnet;
using System.Runtime.InteropServices;
using System.IO;
using WebRadio;

namespace WebRadio.API
{
    static class Session
    {
        public static IntPtr _sessionPtr;

        public static event LoggedInHandler LoggedIn;

        public static void Init(byte[] appkey)
        {
            var loggedInCallbackDelegate = new LoggedInHandler((session,error)=> LoggedIn(session,error));
            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(loggedInCallbackDelegate);

            // Convert structure to C Pointer
            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks, callbacksPtr, true);
            
            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey,0);
            config.application_key_size = appkey.Length;
            config.api_version = libspotify.SPOTIFY_API_VERSION;
            config.user_agent = "WebRadio"; // ToDo change to final name
            config.cache_location = "tmp"; // ToDo change
            config.settings_location = "tmp"; // ToDo
            config.callbacks = callbacksPtr;

            libspotify.sp_session_create(ref config, out _sessionPtr);

        }

        public static void Login(string username, string password)
        {
            libspotify.sp_error err = libspotify.sp_session_login(_sessionPtr, username, password, false, null);
            Console.WriteLine(err);
            int next_timeout;
            libspotify.sp_session_process_events(_sessionPtr, out next_timeout);
        }
    }
}