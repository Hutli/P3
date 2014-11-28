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
        public PlaylistViewModel playlistViewModel;
        private VolumeView volumeView = new VolumeView();
        public ListView listView = new ListView();
        private StackLayout layout = new StackLayout();
        private CurrentVoteView currentVoteView = new CurrentVoteView();

        public PlaylistView()
        {
            Session session = Session.Instance();
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
            if (playlistViewModel == null)
            {
                playlistViewModel = new PlaylistViewModel(venue);
                listView.ItemsSource = playlistViewModel.Results;
                listView.ItemTemplate = new TrackTemplate();
                playlistViewModel.LoadComplete += OnLoadComplete;
            }
            else
            {
                playlistViewModel.GetResults(venue);
            }
            nowPlayingView.GetNowPlaying(venue);
        }

        void OnLoadComplete(){
            if (App.User.Vote != null && !playlistViewModel.Results.Contains(App.User.Vote))
            {
                playlistViewModel.Results.Add(App.User.Vote);
            }
            
            foreach(var e in playlistViewModel.Results)
            {
                var currentVote = App.User.Vote;
                if (currentVote != null && e.ISRC == currentVote.ISRC)
                {
                    listView.SelectedItem = e;
                    break;
                }
            }
            
            
        }
    }
}
