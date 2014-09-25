using System;
using System.Collections.Generic;
using Xamarin.Forms;
using WebAPILib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App
{	
	public partial class Resault : ContentPage
	{	
		private search _search;

		public Resault (string str)
		{
			InitializeComponent ();
			Label lab = this.FindByName<Label> ("text");
			_search = new search (str, SearchType.ALBUM);
			string st = "";
			foreach (var item in _search.Albums) {
				st += item.Artists[0].Name;
				st += "\n";
			}
				
		}

		public void click(object sender, EventArgs args)
		{
			Label lab = this.FindByName<Label> ("text");
			string st = "";
			foreach (var item in _search.Albums) {
				st += item.Artists[0].Name;
				st += "\n";
			}
			lab.Text = st;
		}

	}
}

