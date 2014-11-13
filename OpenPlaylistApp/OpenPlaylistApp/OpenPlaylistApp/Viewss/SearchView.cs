using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WebAPILib;

namespace OpenPlaylistApp
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
            App.search.Clear();
            NewSearch(str);
        }

        public async void NewSearch(string searchStr)
        {
            await Task.Run(() =>
            {
                Search search = new Search(searchStr, SearchType.TRACK);
                foreach (Track item in search.Tracks)
                    App.search.Add(item);
            });

            Result = new SearchResultView();
        }
    }
}
