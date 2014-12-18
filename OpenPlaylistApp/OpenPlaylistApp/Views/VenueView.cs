using System;
using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    public class VenueView : ContentView
    {
        private VenueViewModel venueViewModel;
        private readonly Button checkOutButton = new Button {Text = "Check Out"};
        private readonly StackLayout layout = new StackLayout();
        private readonly ListView listView = new ListView();

        public VenueView()
        {
            GetVenues();
            layout.Children.Add(listView);
            listView.ItemSelected += ItemSelected;
            checkOutButton.Clicked += CheckOutClicked;
            Content = layout;
        }

        private void GetVenues()
        {
            venueViewModel = new VenueViewModel();
            listView.ItemsSource = venueViewModel.Results;
            listView.ItemTemplate = new VenueTemplate();
        }

        private async void CheckOutClicked(object sender, EventArgs e)
        {
            var session = Session.Instance();
            var response = await session.CheckOut(App.User.Venue, App.User);
            if(response == "OK")
            {
                listView.SelectedItem = null;
                App.Home.CheckOut();
                layout.Children.Remove(checkOutButton);
            }
        }

        public async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem is Venue)
            {
                var session = Session.Instance();
                var response = await session.CheckIn((Venue)e.SelectedItem, App.User);
                if(response == "OK" || response == "Already checked in")
                {
                    App.User.Venue = (Venue)e.SelectedItem;
                    layout.Children.Add(checkOutButton);
                }
            }
        }
    }
}