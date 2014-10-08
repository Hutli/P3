using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace TestApp2
{
	public class HomePage : TabbedPage
	{

		public HomePage ()
		{
			this.Title = "Home";
			this.Children.Add(new SearchPage());
		}
	}
}

