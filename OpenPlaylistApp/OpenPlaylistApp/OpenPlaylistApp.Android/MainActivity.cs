using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

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
            App.user = new User(tm.DeviceId);

            SetPage(App.GetMainPage());
        }
    }
}

