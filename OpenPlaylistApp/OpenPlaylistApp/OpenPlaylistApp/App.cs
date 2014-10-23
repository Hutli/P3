using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace OpenPlaylistApp
{
	public class App
	{
        public static HomePage Home;

        public static User user = new User("Heider");
        public static Venue venue = new Venue("HeiderBierHaus", "192.168.0.1:5555");

        public static Page GetMainPage()
        {
            return Home ?? (Home = new HomePage());
        }
	}
}
