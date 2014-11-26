using System;
using System.Linq;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using SpotifyDotNet;

namespace OpenPlaylistServer.Services.Implementation
{
    public class PlaybackService : IPlaybackService
    {
        private Spotify _session;
        private NAudio.Wave.WaveFormat _activeFormat;
        private NAudio.Wave.BufferedWaveProvider _sampleStream;
        private NAudio.Wave.WaveOut _waveOut;
        private PlaylistTrack _currentPlaying;
        private IUserService _userService;

        public PlaybackService(IUserService userService)
        {
            _userService = userService;
            _session = Spotify.Instance;
            if (_session != null)
            {
                _session.MusicDelivery += OnRecieveData;
            }
        }

        //public ObservableCollection<User> Users { get { return _userService.Users; } }

        private void OnRecieveData(int sampleRate, int channels, byte[] frames)
        {
            if (_activeFormat == null)
                _activeFormat = new NAudio.Wave.WaveFormat(sampleRate, 16, channels);

            if (_sampleStream == null)
            {
                _sampleStream = new NAudio.Wave.BufferedWaveProvider(_activeFormat)
                {
                    DiscardOnBufferOverflow = true,
                    BufferLength = 3000000
                };
            }

            if (_waveOut == null)
            {
                _waveOut = new NAudio.Wave.WaveOut();
                _waveOut.Init(_sampleStream);
                _waveOut.Play();
            }
            _session.BufferedBytes = _sampleStream.BufferedBytes;
            _session.BufferedDuration = _sampleStream.BufferedDuration;
            _sampleStream.AddSamples(frames, 0, frames.Length);
            
        }

        public void Play(PlaylistTrack track)
        {
            var spotify = SpotifyLoggedIn.Instance;
            if (spotify != null && track != null)
            {
                spotify.Play(track);
                _currentPlaying = track;
            }
        }

        public void Stop()
        {
            var spotify = SpotifyLoggedIn.Instance;
            if (spotify != null)
            {
                spotify.Stop();
                _currentPlaying = null;
            }
            if (_waveOut == null || _sampleStream == null) return;
            _waveOut.Stop();
            _waveOut = null;
            _sampleStream.ClearBuffer();
            _sampleStream = null;
        }


        public WebAPI.Track GetCurrentPlaying()
        {
            if (_currentPlaying == null) return null;
            WebAPI.Track track = WebAPI.WebAPIMethods.GetTrack(_currentPlaying.Uri);
            track.currentDurationStep = Convert.ToInt32(_session.currentDurationStep.TotalMilliseconds);
            return track;
        }

        public void SetCurrentVolume(object sender, EventArgs e)
        {
            if (_waveOut == null) return;
            _waveOut.Volume = GetCurrentVolume();
        }

        public float GetCurrentVolume()
        {
            if (_userService.Users == null) return 0.5F;
            var totalVolume = _userService.Users.Sum(u => u.Volume);
            return totalVolume/_userService.Users.Count();
        }
    }
}
