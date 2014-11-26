namespace OpenPlaylistServer.Services.Interfaces
{
    public interface IVoteService
    {
        void Vote(string userId, string trackUri);
    }
}
