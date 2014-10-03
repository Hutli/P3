using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace TestApp2
{
	public class HomePage : ContentPage
	{

		public HomePage ()
		{
			var layout = new StackLayout () { Spacing = 0 };
			Label tex = new Label () { Text = "Search for a song." };
			SearchBar search = new SearchBar ();
			search.Text = "dad";
			Command cmd = new Command (delegate() {
				string str = search.Text;
				Navigation.PushModalAsync(new SearchResult(str));
			});
			search.SearchCommand = cmd;
			layout.Children.Add (search);
			layout.Children.Add (tex);
			Content = layout;
		}
	}
}

