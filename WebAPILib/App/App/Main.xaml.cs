using System;
using System.Collections.Generic;
using Xamarin.Forms;

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
			((Button)sender).Text = et.Text;

		}
	}
}

