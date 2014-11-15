using OpenPlaylistApp.ViewModels;
using WebAPILib;
using Xamarin.Forms;
namespace OpenPlaylistApp.Views
{
    public class AlbumView : ContentPage
    {
        public AlbumViewModel ViewModel { get { return BindingContext as AlbumViewModel; } }
        public AlbumView(Album album)
        {
            BindingContext = new AlbumViewModel(album);

            var stack = new StackLayout();

            stack.Children.Add(new Label { Text = "Tracks for " + album.Name + ": ", Font = Font.SystemFontOfSize(NamedSize.Large) });

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            stack.Children.Add(activity);

            var list = new ListView();

            var cell = new DataTemplate(typeof(ImageCell));

            cell.SetBinding(TextCell.TextProperty, "Name");
            cell.SetValue(TextCell.DetailProperty, album.Artists[0].Name);
            cell.SetValue(ImageCell.ImageSourceProperty, album.Images[1].URL);
            list.ItemTemplate = cell;

            list.ItemsSource = ViewModel.Tracks;

            stack.Children.Add(list);

            Content = stack;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.IsBusy = true;
            ViewModel.GetTrack();
        }
    }
}
