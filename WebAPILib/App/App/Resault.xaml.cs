using System;
using System.Collections.Generic;
using Xamarin.Forms;
using WebAPILib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;
using Amib.Threading.Internal;


namespace App
{	
	public partial class Resault : ContentPage
	{	
		private search _search;
		string labl = "";

		public Resault (string str)
		{
			InitializeComponent ();
			Label lab = this.FindByName<Label> ("text");
			lab.Text = "test";
			doStuf ();
			search (str);
		}

		public async void doStuf()
		{
			Label lab = this.FindByName<Label> ("text");
			while (true) {
				await Task.Delay (200);
				lab.Text = labl;
			}

		}

		public async void search(string str)
		{
			Label lab = this.FindByName<Label> ("text");
 			_search = new search (str, SearchType.ALBUM);
			foreach (var item in _search.Albums) {
				await Task.Delay (10);
				labl += item.Artists [0].Name + '\n';
			}
		}

	}
}

