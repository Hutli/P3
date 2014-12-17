using System;
using System.Linq;
using Nancy.Helpers;
using NAudio.Wave;
using OpenPlaylistServer.Services.Interfaces;
using SpotifyDotNet;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Implementation
{
    public class PlaybackService : IPlaybackService
    {
        private WaveFormat _activeFormat;
        private Track _currentPlaying;
        private BufferedWaveProvider _sampleStream;
        private WaveOut _waveOut;
        private readonly Spotify _session;
        private readonly IUserService _userService;

        public PlaybackService(IUserService userService)
        {
            _userService = userService;
            _session = Spotify.Instance;
            if(_session != null)
                _session.MusicDelivery += OnRecieveData;
        }

        public void Play(Track track)
        {
            Console.WriteLine("Play called");
            var spotify = SpotifyLoggedIn.Instance;
            if(spotify != null && track != null)
            {
                Console.WriteLine("Called TrackFromLink");
                var task = spotify.TrackFromLink(track.Uri);
                Console.WriteLine("TrackFromLink has loaded");
                if(task.Exception != null)
                {
                    Console.WriteLine(task.Exception);
                    throw task.Exception;
                }
                task.WhenCompleted(task1 =>
                                   {
                                       //completed
                                       Console.WriteLine("When completed called");
                                       spotify.Play(task1.Result);
                                       Console.WriteLine("After spotify.play called in when completed");
                                       _currentPlaying = track;
                                   }, task1 =>
                                      {
                                          // failed
                                          Console.WriteLine("Error playing back track in playbackservice");
                                      });
            }
        }

        public void Stop()
        {
            var spotifyLoggedIn = SpotifyLoggedIn.Instance;
            if(spotifyLoggedIn != null)
            {
                Console.WriteLine("Stopping song " + _currentPlaying);
                Spotify.Instance.ResetCurrentDurationStep();
                spotifyLoggedIn.Stop();
                _currentPlaying = null;
                Console.WriteLine("Stopped song");
            }
            if(_waveOut == null || _sampleStream == null)
                return;
            _waveOut.Stop();
            _waveOut.Dispose();
            _waveOut = null;
            _sampleStream.ClearBuffer();
            _sampleStream = null;
        }

        public Track GetCurrentPlaying()
        {
            if(_currentPlaying == null)
                return null;

            _currentPlaying.CurrentDurationStep = Convert.ToInt32(_session.CurrentDurationStep.TotalMilliseconds);

            return _currentPlaying;
        }

        public void RefreshCurrentVolume()
        {
            if(_waveOut == null)
                return;
            _waveOut.Volume = GetCurrentVolume();
        }

        public float GetCurrentVolume()
        {
            if(_userService.Users == null || _userService.Users.Count == 0)
                return 0.5F;
            var totalVolume = _userService.Users.Sum(u => u.Volume);
            return totalVolume / _userService.Users.Count();
        }

        private void OnRecieveData(int sampleRate, int channels, byte[] frames)
        {
            if(_activeFormat == null)
                _activeFormat = new WaveFormat(sampleRate, 16, channels);

            if(_sampleStream == null)
                _sampleStream = new BufferedWaveProvider(_activeFormat) {DiscardOnBufferOverflow = true, BufferLength = 3000000};

            if(_waveOut == null)
            {
                _waveOut = new WaveOut();
                _waveOut.DeviceNumber = -1;
                _waveOut.Init(_sampleStream);

                _waveOut.Play();

                _waveOut.Volume = GetCurrentVolume();
            }
            _session.BufferedBytes = _sampleStream.BufferedBytes;
            _session.BufferedDuration = _sampleStream.BufferedDuration;
            _sampleStream.AddSamples(frames, 0, frames.Length);
        }
    }
}