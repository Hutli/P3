namespace OpenPlaylistServer
{
    public interface IPlaybackService
    {
        void Play(PlaylistTrack track);
        void Stop();
        PlaylistTrack GetCurrentPlaying();
        void InfluenceVolume(int volPercent, string userId);
    }
}
