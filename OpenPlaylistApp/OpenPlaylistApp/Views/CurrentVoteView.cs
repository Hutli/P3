using System.Linq;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class CurrentVoteView : ContentView
    {
        private readonly Label _lbl = new Label();

        public CurrentVoteView()
        {
            App.User.VoteChanged += ChangeVote;
            Content = _lbl;
        }

        private void ChangeVote(Track track)
        {
            _lbl.Text = "Your vote: " + track.Name + " on " + track.Album;
        }
    }
}
