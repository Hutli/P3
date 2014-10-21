using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WebApiLib;

namespace openPlaylist
{
    public class SearchView : ContentPage
    {
        public StackLayout layout { get; set; }
        private SearchType searchType;
        public SearchResultView Result
        {
            get
            {
                return ViewModel.Result;
            }
            set
            {
                if (ViewModel.Result != null && layout.Children.Contains(ViewModel.Result)) //Crashes
                {
                    layout.Children.Remove(ViewModel.Result);
                }
                ViewModel.Result = value;
                layout.Children.Add(ViewModel.Result);
            }
        }
        public SearchViewModel ViewModel
        {
            get
            {
                return BindingContext as SearchViewModel;
            }
        }


        public SearchView(SearchType type)
        {
            BindingContext = new SearchViewModel();
            searchType = type;

            layout = new StackLayout();

            var search = new SearchBar();

            search.SearchButtonPressed += search_SearchButtonPressed;
            //search.TextChanged += search_SearchButtonPressed;

            layout.Children.Add(search);

            Content = layout;
        }

        void search_SearchButtonPressed(object sender, EventArgs e)
        {
            string str = ((SearchBar)sender).Text;
            Result = new SearchResultView(str, searchType);
        }
    }
}
