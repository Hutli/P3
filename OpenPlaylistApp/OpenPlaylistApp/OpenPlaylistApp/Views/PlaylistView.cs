using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class PlaylistView : ContentView
    {
        private NowPlayingView nowPlayingView = new NowPlayingView();
        private PlaylistViewModel playlistViewModel;
        private VolumeView volumeView = new VolumeView();
        private ListView listView = new ListView();
        private StackLayout layout = new StackLayout();

        public PlaylistView()
        {
            Session session = Session.Instance();
            listView.ItemSelected += session.ItemSelected; //Vote
            
            App.User.VenueChanged += GetPlaylist;

            layout.Children.Add(nowPlayingView);
            layout.Children.Add(listView);
            layout.Children.Add(volumeView);
            Content = layout;
        }

        async void GetPlaylist(Venue venue)
        {
            playlistViewModel = new PlaylistViewModel(venue);
            listView.ItemsSource = playlistViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();
        }
    }
}
