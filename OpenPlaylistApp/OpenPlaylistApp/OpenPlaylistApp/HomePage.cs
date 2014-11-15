using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using WebAPILib;
using Newtonsoft.Json.Linq;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        NavigationPage detailPage;
        ContentPage browsePage;

        public HomePage()
        {
            Title = "Home";

            App.Playlist = new ObservableCollection<Track>();
            App.Venues = new ObservableCollection<Venue>();
            App.Search = new ObservableCollection<Track>();

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

        async void GetVenues()
        {
            Session session = Session.Instance();
            try
            {
                var str = await session.GetVenues();
                JObject o = JObject.Parse(str);
                foreach (var item in o.Values())
                {
                    App.Venues.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"],(string)item["iconUrl"]));
                }
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

