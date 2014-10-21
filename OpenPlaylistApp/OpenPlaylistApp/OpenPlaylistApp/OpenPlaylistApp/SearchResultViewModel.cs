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
	public class SearchResultModel : BaseViewModel
	{
        private SearchType type;
        public string searchStr { get; set; }
        public search Search { get; set; }
		public ObservableCollection<SpotifyObject> songs { get; set;}
		public SearchResultModel (string str, SearchType type)
		{
			songs = new ObservableCollection<SpotifyObject> ();
            searchStr = str;
            this.type = type;
		}

		private Command loadSongsCommand;

		public Command LoadSongsCommand {
			get { return loadSongsCommand ?? (loadSongsCommand = new Command (ExecuteloadSongsCommand)); }
		}

		public async void ExecuteloadSongsCommand(){
            await Task.Run(() =>
            {
                IsBusy = true;   
                Search = new search(searchStr, type);
                if(type == SearchType.TRACK)
                    foreach (var item in Search.Tracks)
                    {
                        songs.Add(item);
                    }
                if (type == SearchType.ALBUM)
                    foreach (var item in Search.Albums)
                    {
                        songs.Add(item);
                    }
                if (type == SearchType.ARTIST)
                    foreach (var item in Search.Artists)
                    {
                        songs.Add(item);
                    }
                IsBusy = false;
            });
		}

	}
}

