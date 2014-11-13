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

        public static ObservableCollection<Track> playlist;
        public static ObservableCollection<Venue> venues;
        public static ObservableCollection<Track> search;

        public static User user; //Make platform specific, with IMEI, and construct it there

        public static Page GetMainPage()
        {
            return home;
        }
	}
}
