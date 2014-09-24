using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace VotingApp
{
	[Activity (Label = "ListTracksActivity")]			
	public class ListTracksActivity : Activity
	{
		string str;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ListTracks);
			str = Intent.GetStringExtra ("search");

		}
	}
}

