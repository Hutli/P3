using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace OpenPlaylistApp
{
	public class App
	{
        public static PlaylistPage Home;

        public static User user = new User("Empty");
        public static Venue venue = new Venue("Empty","Empty");

        public static Page GetMainPage()
        {
            return Home ?? (Home = new PlaylistPage());
        }
	}
}
