using System;
using Xamarin.Forms;
using System.Collections.Generic;
using WebAPILib;

namespace TestApp2
{
	public class SearchPage : ContentPage
	{
		public SearchViewModel ViewModel
		{
			get { return BindingContext as SearchViewModel; }
		}

		public StackLayout layout = new StackLayout {
			Orientation = StackOrientation.Vertical,
			Padding = new Thickness(0, 8, 0, 8)
		};	

		public SearchPage ()
		{
			this.Title = "Search";

			SearchBar search = new SearchBar ();
			Command cmd = new Command ( delegate () {
				string str = search.Text;
				ShowResults(str);
			});
			search.SearchCommand = cmd;
			layout.Children.Add (search);
			Content = layout;
		}

		public void ShowResults(string str){
			BindingContext = new SearchViewModel (str);
			ViewModel.LoadSongsCommand.Execute (null);

			ListView lst = new ListView ();

			lst.ItemsSource = ViewModel.songs;

			var cell = new DataTemplate(typeof(ImageCell));
			cell.SetBinding (TextCell.TextProperty, "Name");
			cell.SetBinding (TextCell.DetailProperty, "Album.Name");
			cell.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[1].URL");
			lst.ItemTemplate = cell;

			lst.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				try{
					Track selected = ((Track)e.SelectedItem);
					Request.get("http://192.168.1.2:1234/" + selected.URI);
				}catch (Exception efd)
				{
				}
			};

			layout.Children.Add (lst);
		}
	}
}

