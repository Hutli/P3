namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IVoteService
    {
        bool Vote(string userId, string trackUri);
    }
}