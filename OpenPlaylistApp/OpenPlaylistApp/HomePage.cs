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
using System.Windows;

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

        private ToolbarItem tbi1;

        public HomePage()
        {
            Title = "Home";

            playlistView = new PlaylistView();
            searchView = new SearchView();
            venueView = new VenueView();
            checkInView = new CheckInView();

            playlistPage = new ContentPage { Title = "Playlist", Content = playlistView };

            browsePage = new ContentPage { Title = "Browse", Content = searchView };
            venuePage = new ContentPage { Title = "Venues", Content = venueView };
            checkInPage = new ContentPage { Title = "CheckIn", Content = checkInView };

            detailPage = new NavigationPage(playlistPage) { Title = "Playlist", Icon = "Resources/venueIcon.png" };
            
            #if WINDOWS_PHONE
                tbi1 = new ToolbarItem("Add", "Resources/plussign.png", () => BrowseClicked(), 0, 0);
            #else
                tbi1 = new ToolbarItem("Add", "plussign.png", () => BrowseClicked(), 0, 0);
            #endif

            App.User.VenueChanged += CheckedIn;
            App.User.VenueChanged += (Venue v) => this.IsPresented = false;
            App.User.VoteChanged += (Track t) => detailPage.PopToRootAsync();

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1)); // update from server every second
                    if (App.User != null && App.User.Venue != null)
                    {
                        Device.BeginInvokeOnMainThread((() =>
                        {
                            playlistView.GetPlaylist(App.User.Venue);
                        }));
                    }
                }
            });

            #if WINDOWS_PHONE
                venuePage.BackgroundColor = Color.Accent;
            #endif

            Detail = checkInPage;
            Master = venuePage;
        }
        
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            App.User.ScreenHeight = height;
            App.User.ScreenWidth = width;
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
#if WINDOWS_PHONE
                ToolbarItems.Add(tbi1);
#else
                playlistPage.ToolbarItems.Add(tbi1);
#endif
            }
        }

        public void CheckOut()
        {
            Detail = checkInPage;
#if WINDOWS_PHONE
                ToolbarItems.Remove(tbi1);
#else
            playlistPage.ToolbarItems.Remove(tbi1);
#endif
            App.User.Venue = null;
            App.User.Vote = null;
        }
    }
}

