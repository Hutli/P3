using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WebApiLib;

namespace openPlaylist
{
    public class AlbumView : ContentPage
    {
        public AlbumViewModel ViewModel { get { return BindingContext as AlbumViewModel; } }
        public AlbumView(Album album)
        {
            BindingContext = new AlbumViewModel(album);

            var stack = new StackLayout();

            stack.Children.Add(new Label() { Text = "Tracks for " + album.Name + ": ", Font = Font.SystemFontOfSize(NamedSize.Large) });

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            stack.Children.Add(activity);

            var list = new ListView();

            var cell = new DataTemplate(typeof(ImageCell));

            cell.SetBinding(TextCell.TextProperty, "Name");
            cell.SetValue(TextCell.DetailProperty, album.Artists[0].Name);
            cell.SetValue(ImageCell.ImageSourceProperty, album.Images[1].URL);
            list.ItemTemplate = cell;

            list.ItemsSource = ViewModel.Tracks;

            list.ItemSelected += (sender, item) =>
            {
                 PlaylistViewModel.vote = (item.SelectedItem as Track);
                 PlaylistViewModel.Home.GoToPlaylist();
            };

            stack.Children.Add(list);

            Content = stack;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.IsBusy = true;
            ViewModel.getTrack();
        }
    }
}
