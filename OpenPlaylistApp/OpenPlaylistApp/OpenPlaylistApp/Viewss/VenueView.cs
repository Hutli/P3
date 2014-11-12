using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class VenueView : ContentView
    {

        public VenueView()
        {
            var list = new ListView();
            list.ItemTemplate = new VenueTemplate();
            list.ItemsSource = App.venues;
            list.ItemSelected += ItemSelected;
            Content = list;
        }

        public void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Venue)
            {
                App.user.Venue = (Venue)e.SelectedItem;
            }
        }
    }
}
