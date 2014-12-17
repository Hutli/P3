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
        Button checkOutButton = new Button {Text = "Check Out"};
        StackLayout layout = new StackLayout();

        public VenueView()
        {
            GetVenues();
            layout.Children.Add(listView);
            listView.ItemSelected += ItemSelected;
            checkOutButton.Clicked += CheckOutClicked;
            Content = layout;
        }

        void GetVenues()
        {
            venueViewModel = new VenueViewModel();
            listView.ItemsSource = venueViewModel.Results;
            listView.ItemTemplate = new VenueTemplate();
        }

        async void CheckOutClicked(object sender, EventArgs e)
        {
            Session session = Session.Instance();
            var response = await session.CheckOut(App.User.Venue, App.User);
            if (response == "OK")
            {
                listView.SelectedItem = null;
                App.Home.CheckOut();
                layout.Children.Remove(checkOutButton);
            }
        }

        async public void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Venue)
            {
                Session session = Session.Instance();
                var response = await session.CheckIn((Venue)e.SelectedItem, App.User);
                if (response == "OK" || response == "Already checked in")
                {
                    App.User.Venue = (Venue) e.SelectedItem;
                    layout.Children.Add(checkOutButton);
                }
            }
        }
    }
}
