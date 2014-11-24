using System;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class PlaylistView : ContentView
    {
        private NowPlayingView _npv;
        public PlaylistView()
        {
            _npv = new NowPlayingView();

            var list = new ListView {ItemTemplate = new TrackTemplate(), ItemsSource = HomePage.Playlist};

            VolumeView vv = new VolumeView();

            StackLayout layout = new StackLayout { Children = { _npv, list, vv } };

            Content = layout;
        }
    }
}
