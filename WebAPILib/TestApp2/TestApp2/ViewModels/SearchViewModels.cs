using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WebAPILib;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp2
{
	public class SearchViewModel
	{
		private string searchStr;
		public ObservableCollection<Track> songs { get; set;}
		public SearchViewModel (string str)
		{
			songs = new ObservableCollection<Track> ();
			searchStr = str;
			LoadSongsCommand.Execute (null);
		}

		private Command loadSongsCommand;

		public Command LoadSongsCommand {
			get { return loadSongsCommand ?? (loadSongsCommand = new Command (ExecuteloadSongsCommand)); }
		}

		public async void ExecuteloadSongsCommand(){
			await Task.Delay (500);
			search ser = new search (searchStr, SearchType.TRACK);
			foreach (var item in ser.Tracks) {
				songs.Add (item);
			}
		}
	}
}

