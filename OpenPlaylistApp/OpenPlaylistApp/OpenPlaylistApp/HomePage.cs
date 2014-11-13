using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebAPILib;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        NavigationPage detailPage;
        ContentPage playlistPage;
        ContentPage browsePage;
        ContentPage venuePage;

        public HomePage()
        {
            Title = "Home";

            App.playlist = new ObservableCollection<Track>();
            App.venues = new ObservableCollection<Venue>();
            App.search = new ObservableCollection<Track>();
            
            //Testing purpose
            App.venues.Add(new Venue("one", "192.168.1.168"));
            App.venues.Add(new Venue("two", "192.168.1.169"));

            var playlistView = new PlaylistView();
            var browseView = new SearchView();
            var venueView = new VenueView();

            playlistPage = new ContentPage() {Title="PlaylistPage", Content = playlistView };
            browsePage = new ContentPage() { Title = "BrowsePage", Content = browseView };
            venuePage = new ContentPage() {Title="VenuePage", Content = venueView };

            detailPage = new NavigationPage(playlistPage) { Title="DetailPage" };

            Master = venuePage;
            Detail = detailPage;
        }

        public void BrowseClicked(){
            detailPage.PushAsync(browsePage);
        }

        public void BackPressed()
        {
            detailPage.PopAsync();
        }
    }
}

