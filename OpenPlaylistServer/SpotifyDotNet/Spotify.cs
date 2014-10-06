using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using libspotifydotnet;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace SpotifyDotNet
{
    public enum LoginState
    {
        OK,
        UnableToContactServer,
        UserBanned,
        UserNeedsPremium,
        BadUsernameOrPassword
    }

    public class Spotify
    {
        private IntPtr _sessionPtr = IntPtr.Zero;
        private int _nextTimeout;
        private Object _sync = new Object();
        

        public event LogInHandler OnLogInError;
        public event Action<SpotifyLoggedIn> OnLogInSuccess;
        
        public delegate void SearchCompleteHandler(SearchResults results);
        public event MusicDeliveryHandler MusicDelivery;
        public  event TrackEndedDelegate TrackEnded;

        public delegate void NotifyMainDelegate(IntPtr session);
        public delegate void MusicDeliveryHandler(int sample_rate, int channels, byte[] frames);
        public event SearchCompleteHandler SearchComplete;
        private IntPtr _searchComplete = IntPtr.Zero;
        private SearchCompleteDelegate searchCompleteDelegate;
        private delegate void SearchCompleteDelegate(IntPtr search, IntPtr userData);
        
        
        private delegate int MusicDeliveryDelegate(IntPtr session, IntPtr audioFormat, IntPtr frames, int numFrames);
        private delegate void GetAudioBufferStatsDelegate(IntPtr session, IntPtr bufferStats);
        public delegate void TrackEndedDelegate(IntPtr session);
        private delegate void LoggedInDelegate(IntPtr session, libspotify.sp_error err);
        public delegate void LogInHandler(LoginState loginState);

        private MusicDeliveryDelegate musicDeliveryDelegate;
        private LoggedInDelegate loggedInCallbackDelegate;
        private NotifyMainDelegate notifyMainDelegate;
        private TrackEndedDelegate trackEndedDelegate;
        private GetAudioBufferStatsDelegate getAudioBufferStatsDelegate;

        public TimeSpan BufferedDuration { get; set; }
        public int BufferedBytes { get; set; }

        private static readonly Spotify _instance = new Spotify();
        private Task _notifyMainTask;
        public static Spotify Instance
        {
            get
            {
                return _instance;
            }
        }

        protected Spotify() { }

        public void Dispose()
        {
            _notifyMainTask.Dispose();
            lock (_sync)
            {
                libspotify.sp_session_release(_sessionPtr);
            }

            System.GC.SuppressFinalize(this);
        }

        ~Spotify()
        {
            Dispose();
        }
        
        private void Init(byte[] appkey)
        {
            loggedInCallbackDelegate = new LoggedInDelegate((session,error) => {
                LoginState loginState;

                switch(error)
                {
                    case libspotify.sp_error.OK: 
                        // login was successful, so no need to check for anything else. Just signal Success.
                        loginState = LoginState.OK;
                        var spotifyLoggedIn = new SpotifyLoggedIn(ref _sessionPtr, _sync, ref _searchComplete);
                        OnLogInSuccess(spotifyLoggedIn);
                        
                        return;

                    case libspotify.sp_error.BAD_USERNAME_OR_PASSWORD:
                        loginState = LoginState.BadUsernameOrPassword;
                        break;
                    case libspotify.sp_error.UNABLE_TO_CONTACT_SERVER:
                        loginState = LoginState.UnableToContactServer;
                        break;
                    case libspotify.sp_error.USER_NEEDS_PREMIUM:
                        loginState = LoginState.UserNeedsPremium;
                        break;
                    case libspotify.sp_error.USER_BANNED:
                        loginState = LoginState.UserBanned;
                        break;
                    default:
                        throw new Exception("Unknown case " + error);
                }

                OnLogInError(loginState);
                
            });

            searchCompleteDelegate = new SearchCompleteDelegate((IntPtr search, IntPtr userData) =>
            {
                var searchResults = new SearchResults(search);
                SearchComplete(searchResults);
            });
            _searchComplete = Marshal.GetFunctionPointerForDelegate(searchCompleteDelegate);
            
            notifyMainDelegate = new NotifyMainDelegate((IntPtr session) => NotifyMainTest(session));

            musicDeliveryDelegate = new MusicDeliveryDelegate((session, audioFormat, framesPtr, numFrames) =>
            {
                byte[] frames;
                libspotify.sp_audioformat format = (libspotify.sp_audioformat)Marshal.PtrToStructure(audioFormat, typeof(libspotify.sp_audioformat));

                if (numFrames == 0)
                {
                    Console.WriteLine("No frames, may be buffering");
                    return 0;
                }

                // only buffer 5 seconds
                if (BufferedDuration > TimeSpan.FromSeconds(5))
                {
                    frames = new byte[0];
                    numFrames = 0;
                }
                else
                {
                    frames = new byte[numFrames * sizeof(Int16) * format.channels];
                }

                Marshal.Copy(framesPtr, frames, 0, frames.Length);

                if (MusicDelivery != null)
                {
                    MusicDelivery(format.sample_rate,format.channels, frames);
                }
                   
                return numFrames;
            });

            trackEndedDelegate = new TrackEndedDelegate((session) =>
            {
                TrackEnded(session);
            });

            getAudioBufferStatsDelegate = new GetAudioBufferStatsDelegate((session, bufferStatsPtr) =>
            {
                libspotify.sp_audio_buffer_stats bufferStats = (libspotify.sp_audio_buffer_stats)Marshal.PtrToStructure(bufferStatsPtr, typeof(libspotify.sp_audio_buffer_stats));
                
                bufferStats.samples = BufferedBytes / 2;
                bufferStats.stutter = 0;
                
            });

            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(loggedInCallbackDelegate);
            session_callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(notifyMainDelegate);
            session_callbacks.music_delivery = Marshal.GetFunctionPointerForDelegate(musicDeliveryDelegate);
            session_callbacks.get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(getAudioBufferStatsDelegate);
            session_callbacks.end_of_track = Marshal.GetFunctionPointerForDelegate(trackEndedDelegate);
            

            
            // Convert structure to C Pointer
            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks, callbacksPtr, true);
            
            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey,0);
            config.application_key_size = appkey.Length;
            config.api_version = libspotify.SPOTIFY_API_VERSION;
            config.user_agent = "openPlaylist";
            config.cache_location = "tmp"; // ToDo change
            config.settings_location = "tmp"; // ToDo
            config.callbacks = callbacksPtr;

            lock (_sync)
            {
                libspotify.sp_session_create(ref config, out _sessionPtr);
            }

        }

        

        private void NotifyMainTest(IntPtr session)
        {
            _notifyMainTask = new Task(() =>
            {
                ProcessEvents();
             });

            _notifyMainTask.Start();
        }

        public void Login(string username, string password, byte[] appkey)
        {
            Init(appkey);
            lock (_sync)
            {
                libspotify.sp_session_login(_sessionPtr, username, password, false, null);
            }
        }

        internal void ProcessEvents()
        {
            lock (_sync){
                do
                {
                    libspotify.sp_session_process_events(_sessionPtr, out _nextTimeout);
                
                } while (_nextTimeout == 0);
            }
        }

        
        
    }
}