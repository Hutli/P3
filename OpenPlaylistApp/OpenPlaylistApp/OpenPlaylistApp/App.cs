using System.Collections.ObjectModel;
using OpenPlaylistApp.Models;
using WebAPILib;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
	public class App
	{
        public static HomePage Home = new HomePage();

        public static ObservableCollection<Track> Playlist;
        public static ObservableCollection<Venue> Venues;
        public static ObservableCollection<Track> Search;

        public static User User; //Make platform specific, with IMEI, and construct it there

        public static Page GetMainPage()
        {
            return Home;
        }
	}
}
