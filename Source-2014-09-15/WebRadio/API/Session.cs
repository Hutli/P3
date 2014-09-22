using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using libspotifydotnet;
using System.Runtime.InteropServices;
using System.IO;
using WebRadio;

namespace WebRadio.API
{
    static class Session
    {
        public static IntPtr _sessionPtr;
        private static IntPtr _searchComplete;
        public static int _nextTimeout;
        private static Object _sync = new Object();

        public static event LoggedInHandler LoggedIn;
        public static event SearchCompleteHandler SearchComplete;
        //public static event NotifyMainHandler NotifyMain;

        public static void Init(byte[] appkey)
        {
            var loggedInCallbackDelegate = new LoggedInHandler((session,error)=> LoggedIn(session,error));
            var searchCompleteDelegate = new SearchCompleteHandler((IntPtr search, IntPtr userData) => SearchComplete(search, userData));
            var notifyMainDelegate = new NotifyMainHandler((IntPtr session) => NotifyMainTest(session));

            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(loggedInCallbackDelegate);
            session_callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(notifyMainDelegate);

            _searchComplete = Marshal.GetFunctionPointerForDelegate(searchCompleteDelegate);

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

            lock (_sync)
            {
                libspotify.sp_session_create(ref config, out _sessionPtr);
            }

        }

        private static void NotifyMainTest(IntPtr session)
        {
            lock (_sync)
            {
            libspotify.sp_session_process_events(session, out _nextTimeout);
            }
            Console.WriteLine("main notified: next timeout: " + _nextTimeout);
        }

        public static void Login(string username, string password)
        {
            lock (_sync)
            {
                libspotify.sp_session_login(_sessionPtr, username, password, false, null);
            }
            
        }

        public static class Search
        {

            public static IntPtr BeginSearchOnQuery(string query)
            {
                IntPtr queryPointer = Marshal.StringToHGlobalAnsi(query);
                    IntPtr searchPtr = libspotify.sp_search_create(_sessionPtr, queryPointer, 0, 10000, 0, 10, 0, 10, 0, 10, libspotifydotnet.sp_search_type.SP_SEARCH_STANDARD, _searchComplete, IntPtr.Zero);
                    
                    return searchPtr;
            }
        }
    }
}