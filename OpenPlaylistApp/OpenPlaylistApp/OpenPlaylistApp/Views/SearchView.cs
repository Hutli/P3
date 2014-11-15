﻿using System;
using System.Threading.Tasks;
using WebAPILib;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    public class SearchView : ContentView
    {
        public SearchResultView Result;

        public SearchView()
        {
            BindingContext = Result;

            StackLayout layout = new StackLayout();

            var search = new SearchBar();

            NewSearch("dad");

            Result = new SearchResultView();

            search.SearchButtonPressed += search_SearchButtonPressed;
            //search.TextChanged += search_SearchButtonPressed; search as you type, maybe introduce delay

            layout.Children.Add(search);
            layout.Children.Add(Result);

            Content = layout;
        }

        void search_SearchButtonPressed(object sender, EventArgs e)
        {
            string str = ((SearchBar)sender).Text;
            App.Search.Clear();
            NewSearch(str);
        }

        public async void NewSearch(string searchStr)
        {
            await Task.Run(() =>
            {
                Search search = new Search(searchStr, SearchType.Track);
                foreach (Track item in search.Tracks)
                    App.Search.Add(item);
            });

            Result = new SearchResultView();
        }
    }
}
