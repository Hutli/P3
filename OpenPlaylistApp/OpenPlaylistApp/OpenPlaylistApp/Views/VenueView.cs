using OpenPlaylistApp.Models;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    public class VenueView : ContentView
    {

        public VenueView()
        {
            var list = new ListView {ItemTemplate = new VenueTemplate(), ItemsSource = App.Venues};
            list.ItemSelected += ItemSelected;
            Content = list;
        }

        public void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Venue)
            {
                App.User.Venue = (Venue)e.SelectedItem;
            }
        }
    }
}
