using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace TestApp2
{
	public class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			this.Title = "Search";
			var layout = new StackLayout () { Spacing = 0 };
			SearchBar search = new SearchBar ();
			Command cmd = new Command ( delegate () {
				string str = search.Text;
				Navigation.PushModalAsync(new SearchResult(str));
			});
			search.SearchCommand = cmd;
			layout.Children.Add (search);
			Content = layout;
		}
	}
}

