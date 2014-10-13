using libspotifydotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

        internal SpotifyLoggedIn(ref IntPtr _sessionPtr, object _sync ,ref IntPtr _searchComplete)
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
    }
}
