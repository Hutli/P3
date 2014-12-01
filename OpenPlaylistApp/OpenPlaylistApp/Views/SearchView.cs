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
        private CurrentVoteView currentVoteView = new CurrentVoteView();
        private Button nextResultsButton = new Button{Text = "More results"};
        StackLayout layout = new StackLayout();
        ActivityIndicator activity = new ActivityIndicator { IsEnabled = true };

        private SearchViewModel searchViewModel
        {
            get { return BindingContext as SearchViewModel; }
        }

        public SearchView()
        {
            Session session = Session.Instance();

            BindingContext = new SearchViewModel();
            listView.ItemsSource = searchViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();

            activity.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            
            listView.ItemSelected += (sender, args) =>
            {
                //var listview = sender as ListView;
                Track track = listView.SelectedItem as Track;
                track.IsFiltered = true;
                session.ItemSelected(sender, args);                
            }; //Vote
            searchBar.SearchButtonPressed += SearchButtonPressed;

            layout.Children.Add(searchBar);
            layout.Children.Add(activity);
            layout.Children.Add(listView);
            layout.Children.Add(nextResultsButton);
            Content = layout;
        }

        void SearchButtonPressed(object sender, EventArgs e)
        {
            searchViewModel.GetResults(((SearchBar)sender).Text);
            if (nextResultsButton == null) return;
            nextResultsButton.Clicked += nextResultsButton_Clicked;
        }

        void nextResultsButton_Clicked(object sender, EventArgs e) {
            searchViewModel.GetResultsAndAppend(searchBar.Text, searchViewModel.resultCount);
            searchViewModel.resultCount += 20;
        }


        //void SearchButtonPressed(object sender, EventArgs e)
        //{
        //    if (searchViewModel == null)
        //    {
        //        var searchString = ((SearchBar)sender).Text;
        //        searchViewModel = new SearchViewModel(searchString);
        //        listView.ItemsSource = searchViewModel.Results;
        //        listView.ItemTemplate = new TrackTemplate();
        //    }
        //    else
        //        searchViewModel.GetResults(((SearchBar)sender).Text);
        //}
    }
}
