using System.Collections.ObjectModel;
using OpenPlaylistApp.Models;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
	public class App
	{
        public static HomePage Home = new HomePage();

        public static User User; //Make platform specific, with IMEI, and construct it there

        public static Page GetMainPage()
        {
            return Home;
        }
	}
}
