using System;
using System.Linq;
using System.Threading.Tasks;
using OpenPlaylistApp.Models;
using OpenPlaylistApp.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using WebAPI;
using Newtonsoft.Json.Linq;
using OpenPlaylistApp.ViewModels;
using System.Collections.Generic;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        NavigationPage detailPage;
        ContentPage browsePage;

        private PlaylistView _playlistView;

        public static ObservableCollection<Track> Playlist;
        public static ObservableCollection<Venue> Venues;

        public HomePage()
        {
            Title = "Home";

            Playlist = new ObservableCollection<Track>();
            Venues = new ObservableCollection<Venue>();

            _playlistView = new PlaylistView();

            var browseView = new SearchView();
            var venueView = new VenueView();

            ContentPage playlistPage = new ContentPage {Title="PlaylistPage", Content = _playlistView };
            browsePage = new ContentPage { Title = "BrowsePage", Content = browseView };
            ContentPage venuePage = new ContentPage {Title="VenuePage", Content = venueView };

            detailPage = new NavigationPage(playlistPage) { Title="DetailPage" };
			NavigationPage.SetHasNavigationBar (playlistPage, true);
			ToolbarItem tbi = null;
			tbi = new ToolbarItem ("+", null, () => BrowseClicked(), 0, 0);
			ToolbarItems.Add (tbi);
            
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetVenues();
        }

        async void GetVenues()
        {
            Session session = Session.Instance();
            try
            {
                var str = await session.GetVenues();
                JArray v = JArray.Parse(str);
                foreach (var item in v)
                {
                    Venues.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"],(string)item["iconUrl"]));
                }
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

