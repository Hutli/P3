using System;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class PlaylistView : ContentView
    {
        public PlaylistView()
        {
            Button button = new Button { Text = String.Format("+") };

            button.Clicked += (sender, args) => App.Home.BrowseClicked();

            NowPlayingView npv = new NowPlayingView();

            var list = new ListView {ItemTemplate = new TrackTemplate(), ItemsSource = HomePage.Playlist};

            VolumeView vv = new VolumeView();

            StackLayout layout = new StackLayout { Children = { button, npv, list, vv } };

            Content = layout;
        }
    }
}
