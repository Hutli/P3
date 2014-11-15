namespace OpenPlaylistServer
{
    public interface IVoteService
    {
        void Vote(string userId, string trackUri);
    }
}
