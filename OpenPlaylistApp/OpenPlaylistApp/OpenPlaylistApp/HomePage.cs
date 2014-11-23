using System;
using OpenPlaylistApp.Models;
using OpenPlaylistApp.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using WebAPI;
using Newtonsoft.Json.Linq;
using OpenPlaylistApp.ViewModels;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        NavigationPage detailPage;
        ContentPage browsePage;

        public static ObservableCollection<Track> Playlist;
        public static ObservableCollection<Venue> Venues;

        public HomePage()
        {
            Title = "Home";

            Playlist = new ObservableCollection<Track>();
            Venues = new ObservableCollection<Venue>();

            Venue test = new Venue("Heiders", "Lol", "192.168.1.148", "");

            Venues.Add(test);

            var playlistView = new PlaylistView();
            var browseView = new SearchView();
            var venueView = new VenueView();

            ContentPage playlistPage = new ContentPage {Title="PlaylistPage", Content = playlistView };
            browsePage = new ContentPage { Title = "BrowsePage", Content = browseView };
            ContentPage venuePage = new ContentPage {Title="VenuePage", Content = venueView };

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

        void GetVenues()
        {
            Session session = Session.Instance();
            try
            {
                var str = session.GetVenues().Result;
                JObject o = JObject.Parse(str);
                foreach (var item in o.Values())
                {
                    HomePage.Venues.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"],(string)item["iconUrl"]));
                }
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

