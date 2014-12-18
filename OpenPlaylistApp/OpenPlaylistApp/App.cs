using OpenPlaylistApp.Models;
using Xamarin.Forms;

namespace OpenPlaylistApp {
    public class App {
        //Platform specific IMEI, and construct it here to allocate it before delegates, assigned as listeners in HomePage's constructer
        public static User User = new User();
        public static HomePage Home = new HomePage();

        public static Page GetMainPage() {
            return Home ?? new HomePage();
        }
    }
}