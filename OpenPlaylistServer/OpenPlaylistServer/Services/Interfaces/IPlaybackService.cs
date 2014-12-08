using System;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaybackService
    {
        void RefreshCurrentVolume();
        float GetCurrentVolume();
        void Play(Track track);
        void Stop();
        WebAPI.Track GetCurrentPlaying();
    }
}
