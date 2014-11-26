using System;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<String, double> _volumeVotes;  

        public PlaybackService()
        {
            _volumeVotes = new Dictionary<string, double>();
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


        public WebAPI.Track GetCurrentPlaying()
        {
            WebAPI.Track track = WebAPI.WebAPIMethods.GetTrack(_currentPlaying.Uri);
            track.currentDurationStep = Convert.ToInt32(_session.currentDurationStep.TotalMilliseconds);
            return track;
        }

        public void InfluenceVolume(int volPercent, string userId)
        {
            double minimumVol = 0.25;
            double maximumVol = 0.75;
            double vol = volPercent/100;
            if(!_volumeVotes.ContainsKey(userId)) {
                // user has not already voted for volume
                _volumeVotes.Add(userId,vol);
            }
            else
            {
                // update existing volume for user
                _volumeVotes[userId] = vol;
            }

            // calculate average of all votes
            double calcVol = 0;
            double averageVol = _volumeVotes.Values.Average();
            if (averageVol < minimumVol)
            {
                calcVol = minimumVol;
            }
            else if (averageVol > maximumVol)
            {
                calcVol = maximumVol;
            }
            else
            {
                calcVol = averageVol;
            }
            _waveOut.Volume = (float) calcVol;
        }
    }
}
