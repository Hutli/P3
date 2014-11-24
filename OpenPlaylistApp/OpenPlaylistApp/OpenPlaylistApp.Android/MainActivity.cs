using Android.App;
using Android.Content.PM;
using Android.OS;
using OpenPlaylistApp.Models;
using Xamarin.Forms.Platform.Android;
using Android.Telephony;

namespace OpenPlaylistApp.Droid
{
    [Activity(Label = "OpenPlaylist", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            //Assign IMEI
            var tm = (TelephonyManager)GetSystemService(TelephonyService);
            App.User = new User(tm.DeviceId);

			SetPage(App.GetMainPage());
        }
    }
}

