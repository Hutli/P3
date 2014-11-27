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
        SearchViewModel searchViewModel;

        public SearchView()
        {
            Session session = Session.Instance();
            listView.ItemSelected += (sender, args) =>
            {
                var listview = sender as ListView;
                Track track = listView.SelectedItem as Track;
                session.ItemSelected(sender, args);
                if (track != null)
                {
                    //searchViewModel.Results.Add(track);
                }
                
            }; //Vote
            searchBar.SearchButtonPressed += SearchButtonPressed;

            layout.Children.Add(searchBar);
            layout.Children.Add(listView);
            Content = layout;

        }



        void SearchButtonPressed(object sender, EventArgs e)
        {
            if (searchViewModel == null)
            {
                var searchString = ((SearchBar)sender).Text;
                searchViewModel = new SearchViewModel(searchString);
                listView.ItemsSource = searchViewModel.Results;

                listView.ItemTemplate = new TrackTemplate();
            }
            else
                searchViewModel.GetResults(((SearchBar)sender).Text);
        }
    }
}
