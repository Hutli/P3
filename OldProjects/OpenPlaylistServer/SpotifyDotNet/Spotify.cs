using libspotifydotnet;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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
        private IntPtr _searchComplete = IntPtr.Zero;
        private SearchCompleteDelegate _searchCompleteDelegate;
        private delegate void SearchCompleteDelegate(IntPtr search, IntPtr userData);
        private delegate int MusicDeliveryDelegate(IntPtr session, IntPtr audioFormat, IntPtr frames, int numFrames);
        private delegate void GetAudioBufferStatsDelegate(IntPtr session, IntPtr bufferStats);
        private delegate void LoggedInDelegate(IntPtr session, libspotify.sp_error err);
        private delegate void NotifyMainDelegate(IntPtr session);
        private delegate void TrackEndedDelegate(IntPtr session);
        private MusicDeliveryDelegate _musicDeliveryDelegate;
        private LoggedInDelegate _loggedInCallbackDelegate;
        private NotifyMainDelegate _notifyMainDelegate;
        private TrackEndedDelegate _trackEndedDelegate;
        private GetAudioBufferStatsDelegate _getAudioBufferStatsDelegate;
        private static readonly Spotify _instance = new Spotify();
        private Task _notifyMainTask;
        private ManualResetEvent _loggedInResetEvent = new ManualResetEvent(false);
        private SpotifyLoggedIn spotifyLoggedIn;
        private LoginState _loginState;

        public event Action<int, int, byte[]>  MusicDelivery;
        public event Action TrackEnded;
        public event Action<SearchResult> SearchComplete;
        public TimeSpan BufferedDuration { get; set; }
        public int BufferedBytes { get; set; }

        private Spotify() { }

        ~Spotify()
        {
            Dispose();
        }

        public static Spotify Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Init(byte[] appkey)
        {
            _loggedInCallbackDelegate = new LoggedInDelegate((session, error) =>
            {
                LoginState loginState;

                switch (error)
                {
                    case libspotify.sp_error.OK:
                        // login was successful, so no need to check for anything else. Just signal Success.
                        loginState = LoginState.OK;
                        spotifyLoggedIn = new SpotifyLoggedIn(ref _sessionPtr, _sync, ref _searchComplete);
                        break;
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

                _loginState = loginState;
                _loggedInResetEvent.Set();
            });

            _searchCompleteDelegate = new SearchCompleteDelegate((IntPtr search, IntPtr userData) =>
            {
                var searchResults = new SearchResult(search);
                if (SearchComplete != null)
                {
                    SearchComplete(searchResults);
                }
            });
            
            _searchComplete = Marshal.GetFunctionPointerForDelegate(_searchCompleteDelegate);

            _notifyMainDelegate = new NotifyMainDelegate((IntPtr session) => NotifyMain(session));

            _musicDeliveryDelegate = new MusicDeliveryDelegate((session, audioFormat, framesPtr, numFrames) =>
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
                    MusicDelivery(format.sample_rate, format.channels, frames);
                }

                return numFrames;
            });

            _trackEndedDelegate = new TrackEndedDelegate((session) =>
            {
                TrackEnded();
            });

            _getAudioBufferStatsDelegate = new GetAudioBufferStatsDelegate((session, bufferStatsPtr) =>
            {
                libspotify.sp_audio_buffer_stats bufferStats = (libspotify.sp_audio_buffer_stats)Marshal.PtrToStructure(bufferStatsPtr, typeof(libspotify.sp_audio_buffer_stats));

                bufferStats.samples = BufferedBytes / 2;
                bufferStats.stutter = 0;

            });

            libspotify.sp_session_callbacks session_callbacks = new libspotify.sp_session_callbacks();
            session_callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(_loggedInCallbackDelegate);
            session_callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(_notifyMainDelegate);
            session_callbacks.music_delivery = Marshal.GetFunctionPointerForDelegate(_musicDeliveryDelegate);
            session_callbacks.get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(_getAudioBufferStatsDelegate);
            session_callbacks.end_of_track = Marshal.GetFunctionPointerForDelegate(_trackEndedDelegate);



            // Convert structure to C Pointer
            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks, callbacksPtr, true);

            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey, 0);
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

        private void NotifyMain(IntPtr session)
        {
            _notifyMainTask = new Task(() =>
            {
                ProcessEvents();
            });

            _notifyMainTask.Start();
        }

        internal void ProcessEvents()
        {
            lock (_sync)
            {
                do
                {
                    libspotify.sp_session_process_events(_sessionPtr, out _nextTimeout);

                } while (_nextTimeout == 0);
            }
        }

        public void Dispose()
        {
            _notifyMainTask.Dispose();
            lock (_sync)
            {
                libspotify.sp_session_release(_sessionPtr);
            }

            System.GC.SuppressFinalize(this);
        }

        public Task<Tuple<SpotifyLoggedIn,LoginState>> Login(string username, string password, bool rememberMe, byte[] appkey)
        {
            Task<Tuple<SpotifyLoggedIn, LoginState>> t = new Task<Tuple<SpotifyLoggedIn, LoginState>>(() =>
            {
                Init(appkey);
                lock (_sync)
                {
                    libspotify.sp_session_login(_sessionPtr, username, password, rememberMe, null);
                }
                _loggedInResetEvent.WaitOne();
                return new Tuple<SpotifyLoggedIn,LoginState>(spotifyLoggedIn,_loginState);
            });
            t.Start();
            return t;
        }
    }
}
