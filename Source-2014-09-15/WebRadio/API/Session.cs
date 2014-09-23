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
using System.Threading;

namespace WebRadio.API
{
    static class Session
    {
        private static IntPtr _sessionPtr;
        private static IntPtr _searchComplete;
        private static int _nextTimeout;
        private static Object _sync = new Object();

        public static event Action LoggedIn;
        public static event SearchCompleteHandler SearchComplete;
        public static event MusicDeliveryHandler MusicDelivery;
        public static event TrackEndedDelegate TrackEnded; 

        private delegate void SearchCompleteDelegate(IntPtr search, IntPtr userData);
        private delegate int MusicDeliveryDelegate(IntPtr session, IntPtr audioFormat, IntPtr frames, int numFrames);
        private delegate void GetAudioBufferStatsDelegate(IntPtr session, IntPtr bufferStats);
        public delegate void TrackEndedDelegate(IntPtr session);
        private delegate void LoggedInDelegate(IntPtr session, libspotify.sp_error err);
        //public static event NotifyMainHandler NotifyMain;


        public static void Dispose()
        {
            libspotify.sp_session_release(_sessionPtr);
        }
        
        public static void Init(byte[] appkey)
        {
            var loggedInCallbackDelegate = new LoggedInDelegate((session,error) => LoggedIn());
            var searchCompleteDelegate = new SearchCompleteDelegate((IntPtr search, IntPtr userData) => {
                var searchResults = new SearchResults(search);
                SearchComplete(searchResults);
            });
            var notifyMainDelegate = new NotifyMainHandler((IntPtr session) => NotifyMainTest(session));

            var musicDeliveryDelegate = new MusicDeliveryDelegate((session, audioFormat, framesPtr, numFrames) =>
            {
                //Console.WriteLine("Music delivery..");

                byte[] frames;
                libspotify.sp_audioformat format = (libspotify.sp_audioformat)Marshal.PtrToStructure(audioFormat, typeof(libspotify.sp_audioformat));;

                if (numFrames == 0)
                {
                    Console.WriteLine("No frames, may be buffering");
                    return 0;
                }

                // only buffer 5 seconds
                if (Program.sampleStream != null && Program.sampleStream.BufferedDuration > TimeSpan.FromSeconds(5))
                {
                    frames = new byte[0];
                    return 0;
                }
                else
                {
                    frames = new byte[numFrames * sizeof(Int16) * format.channels];
                }

                Marshal.Copy(framesPtr, frames, 0, frames.Length);

                //Console.WriteLine(format.sample_rate);

                MusicDelivery(format, frames);
                return numFrames;
            });

            var trackEndedDelegate = new TrackEndedDelegate((session) =>
            {
                TrackEnded(session);
            });

            var getAudioBufferStatsDelegate = new GetAudioBufferStatsDelegate((session, bufferStatsPtr) =>
            {
                libspotify.sp_audio_buffer_stats bufferStats = (libspotify.sp_audio_buffer_stats)Marshal.PtrToStructure(bufferStatsPtr, typeof(libspotify.sp_audio_buffer_stats));
                if (Program.sampleStream != null)
                {
                    bufferStats.samples = Program.sampleStream.BufferedBytes / 2;
                    bufferStats.stutter = 0;
                }
            });

            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(loggedInCallbackDelegate);
            session_callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(notifyMainDelegate);
            session_callbacks.music_delivery = Marshal.GetFunctionPointerForDelegate(musicDeliveryDelegate);
            session_callbacks.get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(getAudioBufferStatsDelegate);
            session_callbacks.end_of_track = Marshal.GetFunctionPointerForDelegate(trackEndedDelegate);
            

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

        

        public static List<Track> FromLink(String link)
        {
            IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
            IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

            libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);
            List<Track> trackList = new List<Track>(); 
            if (linkType == libspotify.sp_linktype.SP_LINKTYPE_TRACK)
            {
                IntPtr spTrackPtr = libspotify.sp_link_as_track(spLinkPtr);
                trackList.Add(new Track(spTrackPtr));
                return trackList;
            } else if ( linkType == libspotify.sp_linktype.SP_LINKTYPE_PLAYLIST) {
                IntPtr Playlist = libspotify.sp_playlist_create(_sessionPtr, spLinkPtr);
                Thread.Sleep(5000);
                for (int i = 0; i < libspotify.sp_playlist_num_tracks(Playlist); i++)
                    trackList.Add(new Track(libspotify.sp_playlist_track(Playlist, i)));
                return trackList;
            }
            else throw new ArgumentException("Virker pt kun med tracks");
            
        }

        private static void NotifyMainTest(IntPtr session)
        {
            Task.Run(() =>
            {
                ProcessEvents();
             });
            
        }

        public static void Login(string username, string password)
        {
            lock (_sync)
            {
                var err = libspotify.sp_session_login(_sessionPtr, username, password, false, null);
                if (err != libspotify.sp_error.OK)
                {
                    Console.WriteLine(err);
                    throw new LoginException("" + err);
                }
            }
            
        }

        public static void ProcessEvents()
        {
            lock (_sync){
                do
                {
                    libspotify.sp_session_process_events(_sessionPtr, out _nextTimeout);
                
                } while (_nextTimeout == 0);
            }
        }

        public static class Player
        {

            public static void Play(Track track)
            {
                libspotify.sp_error err;

                // process events until all track is loaded
                do
                {
                    err = track.Load(_sessionPtr);
                    Console.WriteLine("player load " + err);
                    ProcessEvents();
                } while (err == libspotify.sp_error.IS_LOADING);
                
                
                Console.WriteLine(track.IsLoaded);
                
                var playErr = libspotify.sp_session_player_play(_sessionPtr, true);
                Console.WriteLine("player play " + playErr);

                var availability = track.GetAvailability(_sessionPtr);
                Console.WriteLine(availability);

                Console.WriteLine("duration: " + track.Duration);
            }
        }

        public static class Search
        {
            public static  void BeginSearchOnQuery(string query)
            {
                IntPtr queryPointer = Marshal.StringToHGlobalAnsi(query);

                IntPtr searchPtr = libspotify.sp_search_create(_sessionPtr, queryPointer, 0, 100, 0, 10, 0, 10, 0, 10,
                    libspotifydotnet.sp_search_type.SP_SEARCH_STANDARD, _searchComplete, IntPtr.Zero);
            }
        }
    }
}