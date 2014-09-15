using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using libspotifydotnet;

namespace WebRadio
{
    class Program
    {
        private static byte[] appkey = File.ReadAllBytes("spotify_appkey.key");

        static void Main(string[] args)
        {
            IntPtr session_callback = new IntPtr();
            /*session_callback.logged_in = &logged_in;
             session_callback.notify_main_thread = &notify_main_thread;
             session_callback.music_delivery = &music_delivery;
             session_callback.metadata_updated = &metadata_updated;
             session_callback.play_token_lost = &play_token_lost;
             session_callback.log_message = NULL;
             session_callback.end_of_track = &end_of_track;
             */

            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.api_version = libspotify.SPOTIFY_API_VERSION;
            config.user_agent = "WebRadio";
            config.application_key_size = sizeof(Int32);
            config.application_key = (IntPtr)appkey;
            config.cache_location = "tmp";
            config.settings_location = "tmp";
            config.callbacks = session_callback;

            IntPtr sessionPtr = new IntPtr();

            libspotify.sp_session_create(ref config, out sessionPtr);

            Console.ReadKey();
        }
    }
}
