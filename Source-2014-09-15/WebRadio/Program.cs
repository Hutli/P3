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

namespace WebRadio
{
    class Program
    {
        static List<WaitHandle> handles = new List<WaitHandle>();

        static ManualResetEvent handle = new ManualResetEvent(false);
        static ManualResetEvent LoginHandler = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Session session = new Session();

            string siaj = session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K");

            Console.Write(siaj);

            Console.ReadKey();
        }

        /*
        static void ownTry()
        {
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
