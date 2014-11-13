using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebAPILib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        async void GetVenues()
        {
            Session session = Session.Instance();
            try
            {
                var str = await session.GetVenues();
                JObject o = JObject.Parse(str);
                foreach (var item in o.Values())
                {
                    App.venues.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"],(string)item["iconUrl"]));
                }
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

