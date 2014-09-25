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

		public Resault (string str)
		{
			InitializeComponent ();
			Label lab = this.FindByName<Label> ("text");
			lab.Text = "test";
		}

		public void testMethod()
		{
			Label lab = this.FindByName<Label> ("text");
			lab.Text = "testMethod";

		}

		public void search(string str)
		{
			Label lab = this.FindByName<Label> ("text");
			_search = new search (str, SearchType.ALBUM);
			string name = _search.Albums[0].Artists[0].Name;
			lab.Text = name;
		}

	}
}

