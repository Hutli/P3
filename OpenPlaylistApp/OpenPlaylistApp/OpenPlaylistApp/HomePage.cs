using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebAPILib;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            Title = "Home";

            App.playlist = new ObservableCollection<Track>();
            App.venues = new ObservableCollection<Venue>();
            
            //Testing purpose
            App.venues.Add(new Venue("one", ""));
            App.venues.Add(new Venue("two", ""));

            var playlistView = new PlaylistView();
            var venueView = new VenueView();

            var playlistPage = new ContentPage() { Content = playlistView };
            var venuePage = new ContentPage() {Title="Pointless", Content = venueView };

            Master = venuePage;
            Detail = playlistPage;
        }
    }
}

