using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Telephony;
using Android.Util;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace OpenPlaylistApp.Droid {
    [Activity(Label = "OpenPlaylist", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity {
        protected override void OnCreate(Bundle bundle) {
            Insights.Initialize("b827463746b0194debd3651572cf7f0b64ad0ab2", ApplicationContext);
            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            //Assign IMEI
            var tm = (TelephonyManager)GetSystemService(TelephonyService);
            App.User.Id = tm.DeviceId;

            SetPage(App.GetMainPage());

            var metrics = new DisplayMetrics();

            App.User.ScreenWidth = metrics.WidthPixels; // does not work
            App.User.ScreenHeight = metrics.HeightPixels;
        }
    }
}