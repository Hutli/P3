using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using libspotifydotnet;

namespace SpotifyDotNet
{
    /// <summary>
    ///     Possible states of the login operation
    /// </summary>
    public enum LoginState
    {
        OK,
        UnableToContactServer,
        UserBanned,
        UserNeedsPremium,
        BadUsernameOrPassword
    }

    /// <summary>
    ///     The main entry point for logging into Spotify.
    ///     For anything other than logging in, use the SpotifyLoggedIn class obtained through <see cref="Login" /> method.
    /// </summary>
    public class Spotify
    {
        private static readonly Spotify _instance = new Spotify();
        private GetAudioBufferStatsDelegate _getAudioBufferStatsDelegate;
        private bool _loggedIn;
        private LoggedInDelegate _loggedInCallbackDelegate;
        private LoggedOutDelegate _loggedOutCallbackDelegate;
        private LoginState _loginState;
        //private Thread _spotifyThread;

        private LogMessageDelegate _logMessageCallback;
        private MusicDeliveryDelegate _musicDeliveryDelegate;
        private NotifyMainDelegate _notifyMainDelegate;
        private IntPtr _searchComplete = IntPtr.Zero;
        private SearchCompleteDelegate _searchCompleteDelegate;
        private bool _sessionCreated;
        private IntPtr _sessionPtr = IntPtr.Zero;
        private SpotifyLoggedIn _spotifyLoggedIn;
        private StartPlayback _startPlayback;
        private StopPlayback _stopPlayback;
        private StreamingError _streamingError;
        private TrackEndedDelegate _trackEndedDelegate;
        //private Task _notifyMainTask;
        private readonly ManualResetEvent _loggedInResetEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _loggedOutWaitHandler = new ManualResetEvent(false);
        private readonly Object _sync = new Object();
        private Spotify() { }

        /// <summary>
        ///     Used to tell how much data is currently buffered elsewhere
        /// </summary>
        public TimeSpan BufferedDuration { private get; set; }

        public TimeSpan CurrentDurationStep { get; private set; }
        private int BufferedFrames { get; set; }

        /// <summary>
        ///     The number of bytes currently buffered elsewhere.
        /// </summary>
        public int BufferedBytes { private get; set; }

        /// <summary>
        ///     Returns the instance of the Spotify class.
        /// </summary>
        public static Spotify Instance
        {
            get { return _instance; }
        }

        /// <summary>
        ///     Delivers audio data after SpotifyLoggedIn.Play(track) is excecuted.
        /// </summary>
        public event Action<int, int, byte[]> MusicDelivery;

        /// <summary>
        ///     Signaled when a track has ended streaming.
        /// </summary>
        public event Action TrackEnded;

        /// <summary>
        ///     Signaled when playback should start
        /// </summary>
        public event Action PlaybackShouldStart;

        /// <summary>
        ///     Signaled when playback should stop
        /// </summary>
        public event Action PlaybackShouldStop;

        /// <summary>
        ///     Called when a search has completed. Occurs after the call to SpotifyLoggedIn.Search(query).
        /// </summary>
        public event Action<SearchResult> SearchComplete;

        ~Spotify() { Dispose(); }

        private void Init(byte[] appkey)
        {
            _loggedInCallbackDelegate = (session, error) =>
                                        {
                                            LoginState loginState;

                                            switch(error)
                                            {
                                                case libspotify.sp_error.OK:
                                                    // login was successful, so no need to check for anything else. Just signal Success.
                                                    loginState = LoginState.OK;
                                                    _loggedIn = true;
                                                    _spotifyLoggedIn = new SpotifyLoggedIn(ref _sessionPtr, _sync, ref _searchComplete);
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
                                        };

            _loggedOutCallbackDelegate = session => _loggedOutWaitHandler.Set();

            _searchCompleteDelegate = (search, userData) =>
                                      {
                                          var searchResults = new SearchResult(search);
                                          if(SearchComplete != null)
                                              SearchComplete(searchResults);
                                      };

            _searchComplete = Marshal.GetFunctionPointerForDelegate(_searchCompleteDelegate);

            _notifyMainDelegate = NotifyMain;

            _musicDeliveryDelegate = (session, audioFormat, framesPtr, numFrames) =>
                                     {
                                         byte[] frames;
                                         var format = (libspotify.sp_audioformat)Marshal.PtrToStructure(audioFormat, typeof(libspotify.sp_audioformat));

                                         if(numFrames == 0)
                                         {
                                             Console.WriteLine("No frames, may be buffering");
                                             return 0;
                                         }

                                         if(BufferedFrames / format.sample_rate > 0)
                                         {
                                             CurrentDurationStep = CurrentDurationStep.Add(new TimeSpan(0, 0, 0, 0, 1000));
                                             // Maybe every 1000 ms? Fits perfectly! :D Source: https://github.com/FrontierPsychiatrist/node-spotify/blob/master/src/callbacks/SessionCallbacks.cc
                                             BufferedFrames = BufferedFrames - format.sample_rate;
                                             Console.WriteLine(CurrentDurationStep);
                                         }

                                         // only buffer 2 seconds
                                         if(BufferedDuration > TimeSpan.FromSeconds(2))
                                         {
                                             frames = new byte[0];
                                             numFrames = 0;
                                         } else
                                             frames = new byte[numFrames * sizeof(Int16) * format.channels];
                                         //frames = new byte[numFrames * sizeof(Int16) * format.channels];

                                         Marshal.Copy(framesPtr, frames, 0, frames.Length);

                                         if(MusicDelivery != null)
                                             MusicDelivery(format.sample_rate, format.channels, frames);

                                         BufferedFrames += numFrames;

                                         return numFrames;
                                     };

            _trackEndedDelegate = session =>
                                  {
                                      ResetCurrentDurationStep();
                                      TrackEnded();
                                  };

            _getAudioBufferStatsDelegate = (session, bufferStatsPtr) =>
                                           {
                                               //Console.WriteLine("get audio buffer stats called");

                                               var t = Task.Run(() =>
                                                                {
                                                                    lock(_sync)
                                                                    {
                                                                        var bufferStats = (libspotify.sp_audio_buffer_stats)Marshal.PtrToStructure(bufferStatsPtr, typeof(libspotify.sp_audio_buffer_stats));
                                                                        var samples = BufferedDuration.Milliseconds * 44.100;
                                                                        bufferStats.samples = (int)Math.Floor(samples);
                                                                        bufferStats.stutter = 0;
                                                                        // copy managed struct to outputPtr
                                                                        //Marshal.StructureToPtr(bufferStats, bufferStatsPtr, true);
                                                                    }
                                                                });

                                               try
                                               {
                                                   t.Wait();
                                               } catch(Exception)
                                               {
                                                   throw;
                                               }

                                               //Console.WriteLine("get audio buffer stats finished ");
                                           };

            _logMessageCallback = (session, message) => Console.WriteLine("Spotify log: " + message);

            PlayTokenLost playTokenLost = session => Console.WriteLine("Play token was lost. Someone logged in to Spotify elsewhere");

            _startPlayback = session =>
                             {
                                 if(PlaybackShouldStart != null)
                                     PlaybackShouldStart();
                             };

            _stopPlayback = session =>
                            {
                                if(PlaybackShouldStop != null)
                                    PlaybackShouldStop();
                            };

            _streamingError = (session, error) => { Console.WriteLine("Streaming error: " + error); };

            var sessionCallbacks = new libspotify.sp_session_callbacks {logged_in = Marshal.GetFunctionPointerForDelegate(_loggedInCallbackDelegate), logged_out = Marshal.GetFunctionPointerForDelegate(_loggedOutCallbackDelegate), metadata_updated = IntPtr.Zero, connection_error = IntPtr.Zero, message_to_user = IntPtr.Zero, notify_main_thread = Marshal.GetFunctionPointerForDelegate(_notifyMainDelegate), music_delivery = Marshal.GetFunctionPointerForDelegate(_musicDeliveryDelegate), play_token_lost = Marshal.GetFunctionPointerForDelegate(playTokenLost), log_message = Marshal.GetFunctionPointerForDelegate(_logMessageCallback), end_of_track = Marshal.GetFunctionPointerForDelegate(_trackEndedDelegate), streaming_error = Marshal.GetFunctionPointerForDelegate(_streamingError), userinfo_updated = IntPtr.Zero, start_playback = Marshal.GetFunctionPointerForDelegate(_startPlayback), stop_playback = Marshal.GetFunctionPointerForDelegate(_stopPlayback), get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(_getAudioBufferStatsDelegate), offline_status_updated = IntPtr.Zero, offline_error = IntPtr.Zero, credentials_blob_updated = IntPtr.Zero, connectionstate_updated = IntPtr.Zero, scrobble_error = IntPtr.Zero, private_session_mode_changed = IntPtr.Zero};

            // Convert structure to C Pointer
            var callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(sessionCallbacks));
            Marshal.StructureToPtr(sessionCallbacks, callbacksPtr, true);

            var config = new libspotify.sp_session_config {application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey, 0), application_key_size = appkey.Length, api_version = libspotify.SPOTIFY_API_VERSION, user_agent = "OpenPlaylist", cache_location = "tmp", settings_location = "tmp", callbacks = callbacksPtr};

            // only 1 sesion can ever be created
            if(!_sessionCreated)
            {
                var error = libspotify.sp_session_create(ref config, out _sessionPtr);
                if(error != libspotify.sp_error.OK || _sessionPtr == IntPtr.Zero)
                {
                    // could not create session
                    Console.WriteLine("Error creating session");
                    throw new InvalidDataException("Error while creating session");
                }

                _sessionCreated = true;
            }
        }

        private void NotifyMain(IntPtr session) { Task.Run(() => ProcessEvents()); }

        private void ProcessEvents()
        {
            lock(_sync)
            {
                var nextTimeout = 0;
                do
                {
                    libspotify.sp_session_process_events(_sessionPtr, out nextTimeout);
                    Console.WriteLine("Process events");
                } while(nextTimeout == 0);
            }
        }

        private void Dispose()
        {
            lock(_sync)
            {
                if(_sessionPtr != IntPtr.Zero)
                {
                    if(_loggedIn)
                    {
                        libspotify.sp_session_logout(_sessionPtr);
                        _loggedOutWaitHandler.WaitOne();
                    }

                    libspotify.sp_session_release(_sessionPtr);
                    // this suggest we need to log out before releasing http://stackoverflow.com/questions/14350355/libspotify-destruction-procedure
                }
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Logs into Spotify
        /// </summary>
        /// <param name="username">Spotify username</param>
        /// <param name="password">Spotify password</param>
        /// <param name="rememberMe">Should login details be remembered for later use?</param>
        /// <param name="appkey">The Spotify appKey retrieved as a registered spotify developer</param>
        /// <returns>
        ///     A tuple with the first element containing the spotifyLoggedIn object.
        ///     If login was unsuccessful spotifyLoggedIn object will be null,
        ///     and the second element of the tuple will contain the error desription.
        /// </returns>
        public Tuple<SpotifyLoggedIn, LoginState> Login(string username, string password, bool rememberMe, byte[] appkey)
        {
            Init(appkey);
            lock(_sync)
            {
                libspotify.sp_session_login(_sessionPtr, username, password, rememberMe, null);
            }
            // wait for the login to happen
            _loggedInResetEvent.WaitOne();
            return new Tuple<SpotifyLoggedIn, LoginState>(_spotifyLoggedIn, _loginState);
        }

        public void ResetCurrentDurationStep() { CurrentDurationStep = TimeSpan.Zero; }

        private delegate void SearchCompleteDelegate(IntPtr search, IntPtr userData);

        private delegate int MusicDeliveryDelegate(IntPtr session, IntPtr audioFormat, IntPtr frames, int numFrames);

        private delegate void GetAudioBufferStatsDelegate(IntPtr session, IntPtr bufferStats);

        private delegate void LoggedInDelegate(IntPtr session, libspotify.sp_error err);

        private delegate void LoggedOutDelegate(IntPtr session);

        private delegate void NotifyMainDelegate(IntPtr session);

        private delegate void TrackEndedDelegate(IntPtr session);

        private delegate void LogMessageDelegate(IntPtr session, String message);

        private delegate void PlayTokenLost(IntPtr session);

        private delegate void StartPlayback(IntPtr session);

        private delegate void StopPlayback(IntPtr session);

        private delegate void StreamingError(IntPtr session, libspotify.sp_error error);
    }
}