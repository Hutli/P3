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
using System.Threading;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        private NavigationPage detailPage;
        public ContentPage browsePage;
        private ContentPage playlistPage;
        public ContentPage venuePage;
        private ContentPage checkInPage;

        private PlaylistView playlistView;
        private SearchView searchView;
        private VenueView venueView;
        private CheckInView checkInView;

        private ToolbarItem tbi;

        public HomePage()
        {
            Title = "Home";

            playlistView = new PlaylistView();
            searchView = new SearchView();
            venueView = new VenueView();
            checkInView = new CheckInView();

            playlistPage = new ContentPage { Title = "PlaylistPage", Content = playlistView };
            browsePage = new ContentPage { Title = "BrowsePage", Content = searchView };
            venuePage = new ContentPage { Title = "VenuePage", Content = venueView };
            checkInPage = new ContentPage { Title = "CheckInPage", Content = checkInView, Padding = 20 };

            detailPage = new NavigationPage(playlistPage) { Title = "DetailPage" };

            App.User.VenueChanged += CheckedIn;
            App.User.VoteChanged += NewData;

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3)); // update from server every second
                    if (App.User != null && App.User.Venue != null)
                    {
                        Device.BeginInvokeOnMainThread((() =>
                        {
                            playlistView.GetPlaylist(App.User.Venue);
                        }));
                    }
                }
            });

            tbi = new ToolbarItem("Add", "plussign.png", () => BrowseClicked(), 0, 0);
            ToolbarItems.Add(tbi);

            Detail = checkInPage;
            Master = venuePage;

            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(10)); // update from server every second
            //        if (App.User != null && App.User.Venue != null)
            //        {
            //            playlistView.GetPlaylist(App.User.Venue);
            //        }
            //    }
            //});
        }

        void NewData(Track track)
        {
            playlistView.GetPlaylist(App.User.Venue);
        }

        public void BrowseClicked()
        {
            detailPage.PushAsync(browsePage);
        }

        public void BackPressed()
        {
            if (App.User.Venue != null)
                detailPage.PopAsync();
        }

        public void CheckedIn(Venue v)
        {
            if (v != null)
            {
                Detail = detailPage;

            }
        }
    }
}

