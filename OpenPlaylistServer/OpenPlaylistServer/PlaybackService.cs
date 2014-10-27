using SpotifyDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
{
    public class PlaybackService : IPlaybackService
    {
        private Spotify _session;
        private NAudio.Wave.WaveFormat _activeFormat;
        private NAudio.Wave.BufferedWaveProvider _sampleStream;
        private NAudio.Wave.WaveOut _waveOut;

        public PlaybackService()
        {
            _session = Spotify.Instance;
            if (_session != null)
            {
                _session.MusicDelivery += OnRecieveData;
            }
        }

        private void OnRecieveData(int sample_rate, int channels, byte[] frames)
        {
            if (_activeFormat == null)
                _activeFormat = new NAudio.Wave.WaveFormat(sample_rate, 16, channels);

            if (_sampleStream == null)
            {
                _sampleStream = new NAudio.Wave.BufferedWaveProvider(_activeFormat);
                _sampleStream.DiscardOnBufferOverflow = true;
                _sampleStream.BufferLength = 3000000;
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
            }
        }

        public void Stop()
        {
            var spotify = SpotifyLoggedIn.Instance;
            if (spotify != null)
            {
                spotify.Stop();
            }
            if (_waveOut != null && _sampleStream != null)
            {
                _waveOut.Stop();
                _waveOut = null;
                _sampleStream.ClearBuffer();
                _sampleStream = null;
            }
        }
    }
}
