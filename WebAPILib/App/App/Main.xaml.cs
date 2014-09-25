using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Threading;


namespace App
{	
	public partial class Main : ContentPage
	{	
		public Main ()
		{
			InitializeComponent ();

		}

		public void onSearch(object sender, EventArgs args)
		{
			Entry et = this.FindByName<Entry> ("txtSearch");
			Resault res = new Resault (et.Text);
			Task t = Task.Factory.StartNew (delegate {
				res.search(et.Text);
			});
			t.ContinueWith (delegate {
				res.testMethod();
			});
			Navigation.PushModalAsync (res);
		}
	}
}

