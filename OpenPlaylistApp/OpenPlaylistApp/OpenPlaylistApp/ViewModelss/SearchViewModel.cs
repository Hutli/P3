using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WebAPILib;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlaylistApp
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(string str)
        {
            App.search.Clear();
            ExecuteloadSongsCommand(str);
        }

        public async void ExecuteloadSongsCommand(string searchStr)
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                Search search = new Search(searchStr, SearchType.TRACK);
                foreach (Track item in search.Tracks)
                    App.search.Add(item);
                IsBusy = false;
            });
        }

    }
}

