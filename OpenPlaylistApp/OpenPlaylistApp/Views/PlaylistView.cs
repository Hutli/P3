using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class PlaylistView : ContentView
    {
        private NowPlayingView nowPlayingView = new NowPlayingView();
        private VolumeView volumeView = new VolumeView();
        public ListView listView = new ListView();
        private StackLayout layout = new StackLayout();
        private CurrentVoteView currentVoteView = new CurrentVoteView();

        private PlaylistViewModel playlistViewModel
        {
            get { return BindingContext as PlaylistViewModel; }
        }

        public PlaylistView()
        {
            Session session = Session.Instance();

            BindingContext = new PlaylistViewModel();
            playlistViewModel.LoadComplete += OnLoadComplete;
            listView.ItemsSource = playlistViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();
            listView.HasUnevenRows = true;


            listView.ItemSelected += session.ItemSelected; //Vote
            
            App.User.VenueChanged += GetPlaylist;

            layout.Children.Add(nowPlayingView);
            layout.Children.Add(currentVoteView);
            layout.Children.Add(listView);
            layout.Children.Add(volumeView);
            Content = layout;
        }


        public void GetPlaylist(Venue venue)
        {
            playlistViewModel.GetResults(venue);
            nowPlayingView.GetNowPlaying(venue);
        }

        void OnLoadComplete(){
            if (App.User.Vote != null && !playlistViewModel.Results.Contains(App.User.Vote))
            {
                //playlistViewModel.Results.Add(App.User.Vote);
            }
            
            foreach(var e in playlistViewModel.Results)
            {
                var currentVote = App.User.Vote;
                if (currentVote != null && e.URI == currentVote.URI)
                {
                    listView.SelectedItem = e;
                    break;
                }
            }
            
            
        }
    }
}
