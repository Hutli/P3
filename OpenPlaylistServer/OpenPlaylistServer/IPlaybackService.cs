namespace OpenPlaylistServer
{
    public interface IPlaybackService
    {
        void Play(PlaylistTrack track);
        void Stop();
        WebAPI.Track GetCurrentPlaying();
        void InfluenceVolume(int volPercent, string userId);
    }
}
