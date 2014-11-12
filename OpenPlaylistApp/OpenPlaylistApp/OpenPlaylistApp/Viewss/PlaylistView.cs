using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPILib;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class PlaylistView : ContentView
    {
        public PlaylistView()
        {
            Button button = new Button { Text = String.Format("+") };

            button.Clicked += (sender, args) => App.home.BrowseClicked();

            NowPlayingView npv = new NowPlayingView();

            var list = new ListView();
            list.ItemTemplate = new TrackTemplate();
            list.ItemsSource = App.playlist;

            VolumeView vv = new VolumeView();

            StackLayout layout = new StackLayout { Children = { button, npv, list, vv } };

            Content = layout;
        }
    }
}
