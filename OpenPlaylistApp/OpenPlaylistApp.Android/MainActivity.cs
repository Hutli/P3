using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.IO;
using Java.Lang;
using OpenPlaylistApp.Models;
using Xamarin.Forms.Platform.Android;
using Android.Telephony;
using Xamarin;

namespace OpenPlaylistApp.Droid
{
    [Activity(Label = "OpenPlaylist", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
			Insights.Initialize ("b827463746b0194debd3651572cf7f0b64ad0ab2", ApplicationContext);
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);

            //Assign IMEI
            var tm = (TelephonyManager)GetSystemService(TelephonyService);
            App.User.Id = tm.DeviceId;
            
			SetPage(App.GetMainPage());

            Android.Util.DisplayMetrics metrics = new Android.Util.DisplayMetrics();

            App.User.ScreenWidth = metrics.WidthPixels; // does not work
            App.User.ScreenHeight = metrics.HeightPixels;
        }
    }
}

