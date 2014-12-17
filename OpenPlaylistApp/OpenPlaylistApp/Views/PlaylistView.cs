using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    internal class PlaylistView : ContentView
    {
        public ListView listView = new ListView();
        private readonly StackLayout layout = new StackLayout();
        private readonly NowPlayingView nowPlayingView = new NowPlayingView();
        private readonly VolumeView volumeView = new VolumeView();

        public PlaylistView()
        {
            var session = Session.Instance();

            BindingContext = new PlaylistViewModel();
            playlistViewModel.LoadComplete += OnLoadComplete;
            listView.ItemsSource = playlistViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();
            listView.HasUnevenRows = true;

            listView.ItemTapped += session.ItemSelected; //Vote

            //App.User.VenueChanged += GetPlaylist;

            layout.Children.Add(nowPlayingView);
            //layout.Children.Add(currentVoteView);
            layout.Children.Add(listView);
            layout.Children.Add(volumeView);
            Content = layout;
        }

        //private CurrentVoteView currentVoteView = new CurrentVoteView();

        private PlaylistViewModel playlistViewModel
        {
            get { return BindingContext as PlaylistViewModel; }
        }

        public void GetPlaylist(Venue venue)
        {
            playlistViewModel.GetResults(venue);
            nowPlayingView.GetNowPlaying(venue);
        }

        private void OnLoadComplete()
        {
            if(App.User.Vote != null && !playlistViewModel.Results.Contains(App.User.Vote))
            {
                //playlistViewModel.Results.Add(App.User.Vote);
            }

            foreach(var e in playlistViewModel.Results)
            {
                var currentVote = App.User.Vote;
                if(currentVote != null && e.Uri == currentVote.Uri)
                {
                    listView.SelectedItem = e;
                    break;
                }
            }
        }
    }
}