using System;
using System.Threading.Tasks;
using OpenPlaylistApp.Models;
using OpenPlaylistApp.Views;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        public ContentPage searchPage;
        public ContentPage venuePage;
        private readonly ContentPage checkInPage;
        private readonly CheckInView checkInView;
        private readonly NavigationPage detailPage;
        private readonly ContentPage playlistPage;
        private readonly PlaylistView playlistView;
        private readonly SearchView searchView;
        private readonly ToolbarItem tbi1;
        private readonly VenueView venueView;

        public HomePage()
        {
            Title = "Home";

            playlistView = new PlaylistView();
            searchView = new SearchView();
            venueView = new VenueView();
            checkInView = new CheckInView();

            playlistPage = new ContentPage {Title = "Playlist", Content = playlistView};

            searchPage = new ContentPage {Title = "Search", Content = searchView};
            venuePage = new ContentPage {Title = "Venues", Content = venueView};
            checkInPage = new ContentPage {Title = "CheckIn", Content = checkInView};

            detailPage = new NavigationPage(playlistPage) {Title = "Playlist", Icon = "Resources/venueIcon.png"};

#if WINDOWS_PHONE
                tbi1 = new ToolbarItem("Add", "Resources/plussign.png", () => SearchClicked(), 0, 0);
            #else
            tbi1 = new ToolbarItem("Add", "plussign.png", () => SearchClicked(), 0, 0);
#endif

            App.User.VenueChanged += CheckedIn;
            App.User.VenueChanged += (Venue v) => IsPresented = false;
            App.User.VoteChanged += (Track t) => detailPage.PopToRootAsync();

            Task.Run(async () =>
                           {
                               while(true)
                               {
                                   await Task.Delay(TimeSpan.FromSeconds(1)); // update from server every second
                                   if(App.User != null && App.User.Venue != null)
                                       Device.BeginInvokeOnMainThread((() => { playlistView.GetPlaylist(App.User.Venue); }));
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

        public void SearchClicked() { detailPage.PushAsync(searchPage); }

        public void BackPressed()
        {
            if(App.User.Venue != null)
                detailPage.PopAsync();
        }

        public void CheckedIn(Venue v)
        {
            if(v != null)
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