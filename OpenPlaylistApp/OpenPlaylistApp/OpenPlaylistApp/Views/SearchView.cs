using System;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;
using OpenPlaylistApp.Models;
using WebAPI;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace OpenPlaylistApp.Views
{
    public class SearchView : ContentView
    {
        SearchBar searchBar = new SearchBar();
        ListView listView = new ListView();
        StackLayout layout = new StackLayout();
        SearchViewModel searchModel;

        public SearchView()
        {
            listView.ItemSelected += ItemSelected;
            searchBar.SearchButtonPressed += search_SearchButtonPressed;

            ActivityIndicator ac = new ActivityIndicator()
            {
                IsEnabled = false,
                IsVisible = false,
                IsRunning = false
            };
            ac.SetBinding(ActivityIndicator.IsRunningProperty, "isBusy");
            ac.SetBinding(ActivityIndicator.IsEnabledProperty, "isBusy");
            ac.SetBinding(ActivityIndicator.IsVisibleProperty, "isBusy");

            layout.Children.Add(searchBar);
            layout.Children.Add(ac);
            layout.Children.Add(listView);
            Content = layout;

            //search.TextChanged += search_SearchButtonPressed; search as you type, maybe introduce delay
        }

        async void search_SearchButtonPressed(object sender, EventArgs e)
        {
            searchModel = new SearchViewModel(((SearchBar)sender).Text);
            searchModel.isBusy = true;
            listView.ItemsSource = searchModel.Results;
            listView.ItemTemplate = new TrackTemplate();
            searchModel.isBusy = false;
        }

        async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Track)) return;
            Track track = (Track)e.SelectedItem;
            Session session = Session.Instance();

            try
            {
                var json = await session.SendVote(App.User.Venue, track, App.User); //TODO vi bruger ikke variablen
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}
