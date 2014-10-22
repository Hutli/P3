using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace openPlaylist
{
    class PlaylistView : TabbedPage
    {
        public PlaylistView()
        {
            Title = "Playlist";
            var stack = new StackLayout() { Spacing = 0 };
            //var label = new Label() { Text = "Current playlist: ", Font = Font.SystemFontOfSize(NamedSize.Large) };
            //stack.Children.Add(label);

            var list = new ListView();

            PlaylistViewModel.TrackSelected += () =>
            {
                list.SelectedItem = PlaylistViewModel.vote;
            };

            list.ItemsSource = PlaylistViewModel.Tracks;
            list.IsEnabled = false;
            var cell = new DataTemplate(typeof(ImageCell));
            cell.SetBinding(TextCell.TextProperty, "Name");
            cell.SetBinding(TextCell.DetailProperty, "Album.Artists[0].Name");
            cell.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[1].URL");
            list.ItemTemplate = cell;

            stack.Children.Add(list);
            this.IsEnabled = false;
            var con = new ContentPage() { Title = "Current Playlist", Content = stack };
            this.Children.Add(con);
        }

    }
}
