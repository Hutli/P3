using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WebAPILib;

namespace OpenPlaylistApp
{

    public class SearchResultView : ContentView
    {
        public SearchResultModel ViewModel
        {
            get
            {
                return BindingContext as SearchResultModel;
            }
        }

        public SearchResultView(string str, SearchType type)
        {
            BindingContext = new SearchResultModel(str, type);

            var layout = new StackLayout() { Spacing = 0 };

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            layout.Children.Add(activity);

            var list = new ListView();

            var cell = new DataTemplate(typeof(ImageCell));

            switch (type)
            {
                case SearchType.TRACK:
                    cell.SetBinding(TextCell.TextProperty, "Name");
                    cell.SetBinding(TextCell.DetailProperty, "Album.Artists[0].Name");
                    cell.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[0].URL");
                    list.ItemTemplate = cell;
                    break;
                case SearchType.ALBUM:
                    cell.SetBinding(TextCell.TextProperty, "Name");
                    cell.SetBinding(TextCell.DetailProperty, "Artists[0].Name");
                    //cell.SetBinding(ImageCell.ImageSourceProperty, "Images[1].URL");
                    list.ItemTemplate = cell;
                    break;
                case SearchType.ARTIST:
                    cell.SetBinding(TextCell.TextProperty, "Name");
                    cell.SetBinding(TextCell.DetailProperty, "Name");
                    //cell.SetBinding(ImageCell.ImageSourceProperty, "Albums[0].Images[1].URL");
                    list.ItemTemplate = cell;
                    break;

            }

            list.ItemsSource = ViewModel.songs;

            list.ItemSelected += list_ItemSelected;

            layout.Children.Add(list);

            Content = layout;

            OnAppearing();

        }

        async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Track)
            {
                Track track = (Track)e.SelectedItem;
                PlaylistViewModel.vote = track;
                Session session = Session.Instance();
                
                try
                {
                    var json = await session.SendVote(App.venue, track, App.user);
                } catch(Exception ex) {
                    //throw new ConnectionToServerFaultedException("An error occured: '{0}'",ex);
                    
                    App.GetMainPage().DisplayAlert("Error",ex.Message, "OK");
                }
            }
            else if (e.SelectedItem is Album)
            {
                var page = new AlbumView(e.SelectedItem as Album);
                PlaylistViewModel.Home.Detail = new NavigationPage(page) { Title = "Album" };
            }
        }

        protected void OnAppearing()
        {
            if (ViewModel == null || ViewModel.IsBusy || ViewModel.songs.Count > 0)
                return;

            ViewModel.LoadSongsCommand.Execute(null);

        }

    }
}
