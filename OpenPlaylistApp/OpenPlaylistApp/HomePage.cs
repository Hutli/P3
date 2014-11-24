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
        private NavigationPage detailPage;
        private ContentPage browsePage;
        private ContentPage playlistPage;
        private ContentPage venuePage;

        private PlaylistView playlistView;
        private SearchView searchView;
        private VenueView venueView;

        public HomePage()
        {
            Title = "Home";

            playlistView = new PlaylistView();
            searchView = new SearchView();
            venueView = new VenueView();

            playlistPage = new ContentPage {Title="PlaylistPage", Content = playlistView };
            browsePage = new ContentPage { Title = "BrowsePage", Content = searchView };
            venuePage = new ContentPage {Title="VenuePage", Content = venueView };

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
    }
}

