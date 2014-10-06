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

    public class Session
    {
        private IntPtr _sessionPtr = IntPtr.Zero;
        private IntPtr _searchComplete = IntPtr.Zero;
        private int _nextTimeout;
        private Object _sync = new Object();
        private bool _isPlaying = false;

        public event LogInHandler OnLogIn;
        public event SearchCompleteHandler SearchComplete;
        public delegate void SearchCompleteHandler(SearchResults results);
        public event MusicDeliveryHandler MusicDelivery;
        public  event TrackEndedDelegate TrackEnded;

        public delegate void NotifyMainDelegate(IntPtr session);
        public delegate void MusicDeliveryHandler(int sample_rate, int channels, byte[] frames);

        private delegate void SearchCompleteDelegate(IntPtr search, IntPtr userData);
        
        private delegate int MusicDeliveryDelegate(IntPtr session, IntPtr audioFormat, IntPtr frames, int numFrames);
        private delegate void GetAudioBufferStatsDelegate(IntPtr session, IntPtr bufferStats);
        public delegate void TrackEndedDelegate(IntPtr session);
        private delegate void LoggedInDelegate(IntPtr session, libspotify.sp_error err);
        public delegate void LogInHandler(LoginState loginState);

        private MusicDeliveryDelegate musicDeliveryDelegate;
        private LoggedInDelegate loggedInCallbackDelegate;
        private SearchCompleteDelegate searchCompleteDelegate;
        private NotifyMainDelegate notifyMainDelegate;
        private TrackEndedDelegate trackEndedDelegate;
        private GetAudioBufferStatsDelegate getAudioBufferStatsDelegate;

        public TimeSpan BufferedDuration { get; set; }
        public int BufferedBytes { get; set; }

        private static readonly Session _instance = new Session();
        private Task _notifyMainTask;
        public static Session Instance
        {
            get
            {
                return _instance;
            }
        }

        private Session() { }

        public void Dispose()
        {
            _notifyMainTask.Dispose();
            lock (_sync)
            {
                libspotify.sp_session_release(_sessionPtr);
            }

            System.GC.SuppressFinalize(this);
        }

        ~Session()
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
                        loginState = LoginState.OK;
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
                OnLogIn(loginState);
                
            });
            searchCompleteDelegate = new SearchCompleteDelegate((IntPtr search, IntPtr userData) => {
                var searchResults = new SearchResults(search);
                SearchComplete(searchResults);
            });
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
            

            _searchComplete = Marshal.GetFunctionPointerForDelegate(searchCompleteDelegate);

            // Convert structure to C Pointer
            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(session_callbacks));
            Marshal.StructureToPtr(session_callbacks, callbacksPtr, true);
            
            libspotify.sp_session_config config = new libspotify.sp_session_config();
            config.application_key = Marshal.UnsafeAddrOfPinnedArrayElement(appkey,0);
            config.application_key_size = appkey.Length;
            config.api_version = libspotify.SPOTIFY_API_VERSION;
            config.user_agent = "SpotifyDotNet"; // ToDo change to final name
            config.cache_location = "tmp"; // ToDo change
            config.settings_location = "tmp"; // ToDo
            config.callbacks = callbacksPtr;

            lock (_sync)
            {
                libspotify.sp_session_create(ref config, out _sessionPtr);
            }

        }

        public Track TrackFromLink(String link)
        {
            IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
            IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

            libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);
            List<Track> trackList = new List<Track>(); 
            if (linkType == libspotify.sp_linktype.SP_LINKTYPE_TRACK)
            {
                IntPtr spTrackPtr = libspotify.sp_link_as_track(spLinkPtr);
                return new Track(spTrackPtr);
            }
            else throw new ArgumentException("URI was not a track URI");
        }

        public List<Track> PlaylistFromLink(String link)
        {
            IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
            IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

            libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);
            List<Track> trackList = new List<Track>();

            if (linkType == libspotify.sp_linktype.SP_LINKTYPE_PLAYLIST)
            {
                IntPtr Playlist = libspotify.sp_playlist_create(_sessionPtr, spLinkPtr);
                for (int i = 0; i < libspotify.sp_playlist_num_tracks(Playlist); i++)
                    trackList.Add(new Track(libspotify.sp_playlist_track(Playlist, i)));
                return trackList;
            }
            else throw new ArgumentException("URI was not a playlist URI");
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

        public void ProcessEvents()
        {
            lock (_sync){
                do
                {
                    libspotify.sp_session_process_events(_sessionPtr, out _nextTimeout);
                
                } while (_nextTimeout == 0);
            }
        }

        public void BeginSearchOnQuery(string query)
            {
                IntPtr queryPointer = Marshal.StringToHGlobalAnsi(query);

                lock (_sync)
                {
                    IntPtr searchPtr = libspotify.sp_search_create(_sessionPtr, queryPointer, 0, 10, 0, 10, 0, 10, 0, 10,
                    libspotifydotnet.sp_search_type.SP_SEARCH_STANDARD, _searchComplete, IntPtr.Zero);
                }
                
            }

        public void Play(Track track){
            if (_isPlaying)
            {
                Stop();
            }

            libspotify.sp_error err;
            // process events until all track is loaded
            do
            {
                err = track.Load(_sessionPtr);
                Console.WriteLine("player load " + err);
                Session.Instance.ProcessEvents();
            } while (err == libspotify.sp_error.IS_LOADING);
                
                
            Console.WriteLine(track.IsLoaded);
                
            var playErr = libspotify.sp_session_player_play(_sessionPtr, true);
            _isPlaying = true;
            Console.WriteLine("player play " + playErr);

            var availability = track.GetAvailability(_sessionPtr);
            Console.WriteLine(availability);

            Console.WriteLine("duration: " + track.Duration);
            }

        public void Stop() {
            libspotify.sp_session_player_unload(_sessionPtr);
            _isPlaying = false;
        }
    }
}