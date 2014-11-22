using System;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;
using OpenPlaylistApp.Models;

namespace OpenPlaylistApp.Views
{
    public class SearchView : ContentView
    {
        SearchResultView srw;
        SearchBar searchBar = new SearchBar();
        StackLayout layout = new StackLayout();

        public SearchView()
        {
            layout.Children.Add(searchBar);
            Content = layout;
            searchBar.SearchButtonPressed += search_SearchButtonPressed;
            //search.TextChanged += search_SearchButtonPressed; search as you type, maybe introduce delay
        }

        void search_SearchButtonPressed(object sender, EventArgs e)
        {
            if (srw != null)
                layout.Children.Remove(srw);
            srw = new SearchResultView(((SearchBar)sender).Text);
            layout.Children.Add(srw);
        }
    }
}
