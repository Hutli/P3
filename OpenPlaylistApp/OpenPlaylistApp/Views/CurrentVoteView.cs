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
            if (track == null || track.Name == null || track.Album == null)
                _lbl.Text = string.Empty;
            else
            {
                _lbl.Text = "Your vote: " + track.Name + " on " + track.Album;
            }
        }
    }
}
