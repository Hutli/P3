using System;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;

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

            
        }

        void search_SearchButtonPressed(object sender, EventArgs e)
        {
            layout.Children.Remove(Result);
            var searchVM = new SearchViewModel(((SearchBar)sender).Text);
            layout.Children.Add(Result);
            Content.Focus();
        }
    }
}
