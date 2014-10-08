using System;
using Xamarin.Forms;
using System.Collections.Generic;
using WebAPILib;
using System.Collections.ObjectModel;

namespace TestApp2
{
	public class SearchPage : ContentPage
	{
		public SearchViewModel ViewModel
		{
			get { return BindingContext as SearchViewModel; }
		}
			
		public SearchPage ()
		{
			this.Title = "Search";
			StackLayout layout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(0, 8, 0, 8)
			};	

			SearchBar search = new SearchBar ();
			ListView lst = new ListView ();

			var cell = new DataTemplate(typeof(ImageCell));
			cell.SetBinding (TextCell.TextProperty, "Name");
			cell.SetBinding (TextCell.DetailProperty, "Album.Name");
			cell.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[1].URL");
			lst.ItemTemplate = cell;

			Command cmd = new Command ( delegate () {
				string str = search.Text;
				lst.ItemsSource = getResults(str);
			});

			search.SearchCommand = cmd;
			layout.Children.Add (search);
			layout.Children.Add (lst);

			lst.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				try{
					Track selected = ((Track)e.SelectedItem);
					Request.get("http://192.168.1.2:1234/" + selected.URI);
				}catch (Exception efd)
				{
				}
			};

			Content = layout;
		}

		private ObservableCollection<Track> getResults(string str){
			BindingContext = new SearchViewModel (str);
			ViewModel.LoadSongsCommand.Execute (null);
			return ViewModel.songs;
		}
	}
}

