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
		private string _searchString;

		public Resault (string str)
		{
			InitializeComponent ();
			_searchString = str;
			Label lab = this.FindByName<Label> ("text");
			search search = new search (_searchString , SearchType.TRACK);
			string st = "";
			foreach (var item in search.tracks) {
				st += item.Name;
				st += "\n";
			}
			lab.Text = st;
		}



	}
}

