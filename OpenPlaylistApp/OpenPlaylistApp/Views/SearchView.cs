using System;
using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views {
    public class SearchView : ContentView {
        private readonly ActivityIndicator activity = new ActivityIndicator {
            IsEnabled = true
        };

        private readonly StackLayout layout = new StackLayout();
        private readonly ListView listView = new ListView();
        //private CurrentVoteView currentVoteView = new CurrentVoteView();
        private readonly Button nextResultsButton = new Button {
            Text = "More results"
        };

        private readonly SearchBar searchBar = new SearchBar();

        public SearchView() {
            var session = Session.Instance();

            BindingContext = new SearchViewModel();
            listView.ItemsSource = searchViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();
            listView.HasUnevenRows = true;

            activity.SetBinding(IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            listView.ItemTapped += (sender, args) => {
                var track = listView.SelectedItem as Track;
                if(track.IsFiltered) {
                    App.GetMainPage()
                       .DisplayAlert("Track Unavalable",
                                     "Track is unfortunately filtered and not available at this venue",
                                     "OK",
                                     "Cancel");
                } else
                    session.ItemSelected(sender, args);
            }; //Vote
            searchBar.SearchButtonPressed += SearchButtonPressed;

            layout.Children.Add(searchBar);
            layout.Children.Add(activity);
            layout.Children.Add(listView);
            Content = layout;
        }

        private SearchViewModel searchViewModel {
            get {return BindingContext as SearchViewModel;}
        }

        private void SearchButtonPressed(object sender, EventArgs e) {
            searchViewModel.GetResults(((SearchBar)sender).Text);
            if(nextResultsButton == null)
                return;
            layout.Children.Add(nextResultsButton);
            nextResultsButton.Clicked += nextResultsButton_Clicked;
        }

        private void nextResultsButton_Clicked(object sender, EventArgs e) {
            searchViewModel.resultCount += 20;
            searchViewModel.GetResultsAndAppend(searchBar.Text, searchViewModel.resultCount);
        }
    }
}