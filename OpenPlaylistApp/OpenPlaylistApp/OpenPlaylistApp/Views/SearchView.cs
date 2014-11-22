using System;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;
using OpenPlaylistApp.Models;

namespace OpenPlaylistApp.Views
{
    public class SearchView : ContentView
    {
        public SearchResultView Result;
        SearchBar search = new SearchBar();
        StackLayout layout = new StackLayout();

        public SearchView()
        {
            BindingContext = Result;
            Result = new SearchResultView();
            layout.Children.Add(search);
            layout.Children.Add(Result);
            Content = layout;
            search.SearchButtonPressed += search_SearchButtonPressed;
            //search.TextChanged += search_SearchButtonPressed; search as you type, maybe introduce delay
            Venue test = new Venue("Heiders", "Lol", "192.168.1.148", "");

            HomePage.Venues.Add(test);
        }

        void search_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchView = new SearchResultView(((SearchBar)sender).Text);
            Result = searchView;
        }
    }
}
