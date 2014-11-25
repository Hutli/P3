using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    public class VenueView : ContentView
    {
        VenueViewModel venueViewModel;
        ListView listView = new ListView();

        public VenueView()
        {
            GetVenues();
            listView.ItemSelected += ItemSelected;
            Content = listView;
        }

        void GetVenues()
        {
            venueViewModel = new VenueViewModel();
            listView.ItemsSource = venueViewModel.Results;
            listView.ItemTemplate = new VenueTemplate();
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
