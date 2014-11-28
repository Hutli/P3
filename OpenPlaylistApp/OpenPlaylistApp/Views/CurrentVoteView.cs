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
            _lbl.Text = "";
            App.User.VoteChanged += ChangeVote;
            Content = _lbl;
        }

        private void ChangeVote(Track track)
        {
            var artist = track.Album.Artists.FirstOrDefault();
            if (artist != null)
                _lbl.Text = "Your vote: " + track.Name + " - " + artist.Name;
        }
    }
}
