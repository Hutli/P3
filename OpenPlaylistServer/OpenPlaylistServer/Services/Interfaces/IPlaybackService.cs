using System;
using OpenPlaylistServer.Models;
using WebAPI;

namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IPlaybackService
    {
        void SetCurrentVolume(object sender, EventArgs e);
        float GetCurrentVolume();
        void Play(Track track);
        void Stop();
        WebAPI.Track GetCurrentPlaying();
    }
}
