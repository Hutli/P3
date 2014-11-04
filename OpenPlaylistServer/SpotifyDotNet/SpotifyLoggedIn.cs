using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyDotNet
{
    public class SpotifyLoggedIn
    {
        private IntPtr _searchComplete;
        private object _sync;
        private IntPtr _sessionPtr;
        private static SpotifyLoggedIn _instance;
        private bool _isPlaying = false;

        private SpotifyLoggedIn() { }

        internal SpotifyLoggedIn(ref IntPtr _sessionPtr, object _sync, ref IntPtr _searchComplete)
        {
            _instance = this;
            this._searchComplete = _searchComplete;
            this._sync = _sync;
            this._sessionPtr = _sessionPtr;
        }

        public static SpotifyLoggedIn Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new NullReferenceException("Not logged into Spotify");
                }
                return _instance;
            }
        }

        public void Search(string query)
        {
            IntPtr queryPointer = Marshal.StringToHGlobalAnsi(query);

            lock (_sync)
            {
                IntPtr searchPtr = libspotify.sp_search_create(_sessionPtr, queryPointer, 0, 10, 0, 10, 0, 10, 0, 10,
                libspotifydotnet.sp_search_type.SP_SEARCH_STANDARD, _searchComplete, IntPtr.Zero);
            }
        }

        public void Play(Track track)
        {
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
                Spotify.Instance.ProcessEvents();
            } while (err == libspotify.sp_error.IS_LOADING);


            Console.WriteLine(track.IsLoaded);

            var playErr = libspotify.sp_session_player_play(_sessionPtr, true);
            _isPlaying = true;
            Console.WriteLine("player play " + playErr);

            var availability = track.GetAvailability(_sessionPtr);
            Console.WriteLine(availability);

            Console.WriteLine("duration: " + track.Duration);
        }

        public void Stop()
        {
            libspotify.sp_session_player_unload(_sessionPtr);
            _isPlaying = false;
        }

        public Task<Track> TrackFromLink(String link)
        {
            var t = Task.Run<Track>(() =>
            {
                IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
                IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

                if (spLinkPtr == IntPtr.Zero)
                {
                    throw new ArgumentException("URI was not a track URI");
                }
                libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);

                if (linkType == libspotify.sp_linktype.SP_LINKTYPE_TRACK)
                {
                    IntPtr trackLinkPtr = libspotify.sp_link_as_track(spLinkPtr);
                    //libspotify.sp_link_release(spLinkPtr);

                    return new Track(trackLinkPtr);
                }
                else
                {
                    //libspotify.sp_link_release(spLinkPtr);
                    throw new ArgumentException("URI was not a track URI");
                }
            });

            return t;
        }

        internal IntPtr TrackUriToIntPtr(string link)
        {
            IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
            IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

            libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);

            if (linkType == libspotify.sp_linktype.SP_LINKTYPE_TRACK)
            {
                return libspotify.sp_link_as_track(spLinkPtr);
            }
            else throw new ArgumentException("URI was not a track URI");
        }

        public Task<List<Track>> PlaylistFromLink(String link)
        {
            var t = Task.Run<List<Track>>(() =>
            {

                lock (_sync)
                {
                    IntPtr linkPtr = Marshal.StringToHGlobalAnsi(link);
                    IntPtr spLinkPtr = libspotify.sp_link_create_from_string(linkPtr);

                    libspotify.sp_linktype linkType = libspotify.sp_link_type(spLinkPtr);
                    List<Track> trackList = new List<Track>();

                    if (linkType == libspotify.sp_linktype.SP_LINKTYPE_PLAYLIST)
                    {
                        IntPtr playlistPtr = libspotify.sp_playlist_create(_sessionPtr, spLinkPtr);

                        // wait until playlist is loaded including metadata
                        while (libspotify.sp_playlist_is_loaded(playlistPtr) == false)
                        {
                            Task.Delay(2);
                        }

                        int numTracks = libspotify.sp_playlist_num_tracks(playlistPtr);

                        foreach (var i in Enumerable.Range(0, numTracks))
                        {
                            var trackPtr = libspotify.sp_playlist_track(playlistPtr, i);
                            var track = new Track(trackPtr);
                            trackList.Add(track);
                        }
                    }
                    else throw new ArgumentException("URI was not a playlist URI");

                    return trackList;
                }
            });

            return t;
        }
    }
}
