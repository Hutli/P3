using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WebAPILib;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
	public class App
	{
        public static HomePage home = new HomePage();
        public static SearchView search = new SearchView(SearchType.TRACK);

        public static ObservableCollection<Track> playlist;
        public static ObservableCollection<Venue> venues;

        public static User user; //Make this platform specific, with IMEI
        public static Venue venue; //Venue checked in at

        public static Page GetMainPage()
        {
            return home;
        }

        public static Page GetBrowsePage()
        {
            return search;
        }
	}
}
