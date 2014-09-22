using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WebRadio.API;
using libspotifydotnet;
using System.Runtime.InteropServices;
using System.Threading;

namespace WebRadio {
    public delegate void LoggedInHandler(IntPtr session, libspotify.sp_error error);
    public delegate void SearchCompleteHandler(IntPtr search, IntPtr userData);
    public delegate void NotifyMainHandler(IntPtr session);

    class Program {

        private static ManualResetEvent _searchCompleteSignal;
        private static bool _quit;
        public static byte[] appkey;

        private static IntPtr _searchPtr;


        static void Main(string[] args) {
            appkey = File.ReadAllBytes("spotify_appkey.key");

            Session.LoggedIn += new LoggedInHandler(loginTest);
            Session.SearchComplete += new SearchCompleteHandler(searchCompleteTest);
            
            try
            {
                Session.Init(appkey);
                Session.Login("jensstaermose@hotmail.com", "pass");
                _searchPtr = Session.Search.BeginSearchOnQuery("summertime");
                _searchCompleteSignal = new ManualResetEvent(false);
            }
            catch {}
     
                do
                {
                    libspotify.sp_session_process_events(Session._sessionPtr, out Session._nextTimeout);
                } while (Session._nextTimeout == 0);
        }

        private static void loginTest(IntPtr session, libspotify.sp_error error)
        {
            Console.WriteLine("Login:" + error);
        }

        private static void searchCompleteTest(IntPtr searchPtr, IntPtr userdata)
        {
            int num = libspotify.sp_search_num_tracks(searchPtr);
            IntPtr track = libspotify.sp_search_track(searchPtr, 0);
            IntPtr trackname = libspotify.sp_track_name(track);
            string name = Marshal.PtrToStringAnsi(trackname);

            Console.WriteLine("SearchComplete virker, name of track " + name);
        }

        private static void notifyMainTest(IntPtr session)
        {
            Console.WriteLine("Main notified");
        }


        /*
        static void ownTry() {
            var loggedInCallbackDelegate = new logged_in_delegate(logged_in);
            var notifyMainCallbackDelegate = new notify_main_delegate(notifyMain);


            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(loggedInCallbackDelegate);
            session_callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(notifyMainCallbackDelegate);
             session_callback.music_delivery = &music_delivery;
             session_callback.metadata_updated = &metadata_updated;
             session_callback.play_token_lost = &play_token_lost;
             session_callback.log_message = NULL;
             session_callback.end_of_track = &end_of_track;
            

            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks, callbacksPtr, true);

            byte[] appkey = File.ReadAllBytes("spotify_appkey.key");

            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.api_version = libspotify.SPOTIFY_API_VERSION;
            config.user_agent = "WebRadio";
            config.application_key_size = appkey.Length;
            config.application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey, 0);
            config.cache_location = "tmp";
            config.settings_location = "tmp";
            config.callbacks = callbacksPtr;

            IntPtr sessionPtr;
            Console.WriteLine("Creating session");
            var sp_err = libspotify.sp_session_create(ref config, out sessionPtr);

            if (sp_err != libspotify.sp_error.OK)
            {
                Console.WriteLine("Kunne ikke få session");
                Environment.Exit(-1);
            }


            handles.Add(handle);
            handles.Add(LoginHandler);

            var sp_login_err = libspotify.sp_session_login(sessionPtr, "jensstaermose@hotmail.com", "34AKPAKCRE77K", false, null);

            Console.WriteLine("At readLine");
            Console.ReadKey();
        }

        private static void search_complete(IntPtr result, IntPtr userDataPtr)
        {
            Console.WriteLine("Done searching");
            handle.Set();
        }

        private static void logged_in(IntPtr sessionPtr, libspotify.sp_error error)
        {
            if (error != libspotify.sp_error.OK)
            {
                Console.WriteLine("Error logging in: callback: " + error);
            }
            else
            {
                Console.WriteLine("Callback: logged in without error");
                var searchQuery = "Rolling stones";

                var searchCallback = new search_complete_cb_delegate(search_complete);
                var searchCallbackPtr = Marshal.GetFunctionPointerForDelegate(searchCallback);
                var searchPtr = libspotify.sp_search_create(sessionPtr, Marshal.StringToHGlobalAuto(searchQuery), 0, 1, 0, 1, 0, 1, 0, 1,
                libspotifydotnet.sp_search_type.SP_SEARCH_STANDARD, searchCallbackPtr, IntPtr.Zero);
            }

            LoginHandler.Set();

        }

        private static void notifyMain(IntPtr sessionPtr)
        {
            Console.WriteLine("notifyMain called");
            int next_timeout;
            libspotify.sp_session_process_events(sessionPtr, out next_timeout);
        }

        public delegate void search_complete_cb_delegate(IntPtr result, IntPtr userDataPtr);
        public delegate void logged_in_delegate(IntPtr sessionPtr, libspotify.sp_error error);
        public delegate void notify_main_delegate(IntPtr sessionPtr);
        */
    }
}
