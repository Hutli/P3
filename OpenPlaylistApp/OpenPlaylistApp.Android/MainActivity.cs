using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.IO;
using Java.Lang;
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

            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                throw args.Exception;
            };

            

            //Assign IMEI
            var tm = (TelephonyManager)GetSystemService(TelephonyService);
            App.User.Id = tm.DeviceId;

            Android.Util.DisplayMetrics metrics = new Android.Util.DisplayMetrics();

            App.User.ScreenWidth = metrics.WidthPixels; // does not work
            App.User.ScreenHeight = metrics.HeightPixels;

			SetPage(App.GetMainPage());

            
        }
    }
}

