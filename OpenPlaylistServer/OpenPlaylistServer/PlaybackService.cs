using SpotifyDotNet;

namespace OpenPlaylistServer
{
    public class PlaybackService : IPlaybackService
    {
        private Spotify _session;
        private NAudio.Wave.WaveFormat _activeFormat;
        private NAudio.Wave.BufferedWaveProvider _sampleStream;
        private NAudio.Wave.WaveOut _waveOut;
        private PlaylistTrack _currentPlaying;

        public PlaybackService()
        {
            _session = Spotify.Instance;
            if (_session != null)
            {
                _session.MusicDelivery += OnRecieveData;
            }
        }

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


        public PlaylistTrack GetCurrentPlaying()
        {
            return _currentPlaying;
        }
    }
}
