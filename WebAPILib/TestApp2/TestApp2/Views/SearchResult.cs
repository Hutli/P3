using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPILib;

namespace TestApp2
{
	public class SearchResult : ContentPage
	{
//		private bool active = true;
		public SearchViewModel ViewModel
		{
			get { return BindingContext as SearchViewModel; }
		}


		public SearchResult (string str)
		{
			var layout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(0, 8, 0, 8)
			};	


			BindingContext = new SearchViewModel (str);
			ViewModel.LoadSongsCommand.Execute (null);


			layout.Children.Add (new Label () { Text = "Results for \"" + str + "\": ", Font = Font.SystemFontOfSize(NamedSize.Large) });
			ListView lst = new ListView ();

			lst.ItemsSource = ViewModel.songs;

			var cell = new DataTemplate(typeof(ImageCell));
			cell.SetBinding (TextCell.TextProperty, "Name");
			cell.SetBinding (TextCell.DetailProperty, "Album.Name");
			cell.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[0].URL");
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

			Content = layout;
		}
	}
}

