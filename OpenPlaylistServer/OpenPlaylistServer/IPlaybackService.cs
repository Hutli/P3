using System;
namespace OpenPlaylistServer
{
    public interface IPlaybackService
    {
        void SetCurrentVolume(object sender, EventArgs e);
        float GetCurrentVolume();
        void Play(PlaylistTrack track);
        void Stop();
        WebAPI.Track GetCurrentPlaying();
    }
}
