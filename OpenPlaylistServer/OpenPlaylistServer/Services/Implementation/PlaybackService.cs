using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using Nancy.Helpers;
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
        private BinaryWriter Writer = null;

        public PlaybackService(IUserService userService)
        {
            _userService = userService;
            _session = Spotify.Instance;
            if (_session != null)
            {
                _session.MusicDelivery += OnRecieveData;
            }
            var mp = new MediaPlayer();
            mp.Open(new Uri("http://www.wavsource.com/snds_2014-11-23_3967152387108926/people/comedians/allen_arrogh.wav"));
            mp.Play();
            //Thread.Sleep(1000);
            //Writer = new BinaryWriter(File.OpenWrite("musikPCM"));
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
                _waveOut.DeviceNumber = -1;
                _waveOut.Init(_sampleStream);
                
                _waveOut.Play();
            }
            _session.BufferedBytes = _sampleStream.BufferedBytes;
            _session.BufferedDuration = _sampleStream.BufferedDuration;
            _sampleStream.AddSamples(frames, 0, frames.Length);
            //Writer.Write(frames);
            //SystemSounds.Beep.Play();
        }

        public async void Play(PlaylistTrack track)
        {
            var spotify = SpotifyLoggedIn.Instance;
            if (spotify != null && track != null)
            {
                var task = spotify.TrackFromLink(track.URI);
                if (task.Exception != null)
                {
                    Console.WriteLine(task.Exception);
                    throw task.Exception;
                }
                task.WhenCompleted(task1 =>
                {
                    //completed
                    spotify.Play(task1.Result);
                    _currentPlaying = track;
                }, task1 =>
                {
                    // failed
                    Console.WriteLine("Faiiiiiiiiiiiiiiiiiiiled");
                });
               
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
            _waveOut.Dispose();
            _waveOut = null;
            _sampleStream.ClearBuffer();
            _sampleStream = null;
            //Writer.Flush();
            //Writer.Close();
            
        }


        public WebAPI.Track GetCurrentPlaying()
        {
            if (_currentPlaying == null) return null;

            
            WebAPI.Track track = WebAPI.WebAPIMethods.GetTrack(_currentPlaying.URI);
            track.CurrentDurationStep = Convert.ToInt32(_session.CurrentDurationStep.TotalMilliseconds);

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
