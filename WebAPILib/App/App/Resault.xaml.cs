using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

	public class clsEmp
	{
		public clsEmp(string firstname, string lastname)
		{
			// TODO: Complete member initialization
			this.FirstName = firstname;
			this.LastName = lastname;
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public partial class Resault : ContentPage
	{	
		private search _search;
		public ObservableCollection<Track> _tracks;
		public ObservableCollection<clsEmp> Names = new ObservableCollection<clsEmp>();
		private string _lab = "";



		public Resault (string str)
		{
			InitializeComponent ();
			search (str);


			Request.get ("http://192.168.3.242:1234/" + _tracks[0].URI);



			Names.Add(new clsEmp("Jitnedra", "Jadav"));
			Names.Add(new clsEmp("Bhupat", "Jadav"));
			Names.Add(new clsEmp("Rakesh", "Jadav"));
			Names.Add(new clsEmp("Kamlesh", "Jadav"));
			Names.Add(new clsEmp("Rajesh", "Jadav"));
			Names.Add(new clsEmp("Manoj", "Jadav"));


		}

		public async void doStuf()
		{
			Label lb = this.FindByName<Label> ("test");
			while (true) {
				await Task.Delay (200);
				//lb.Text = _lab;		
			}

		}


		public void search(string str)
		{
			_search = new search (str, SearchType.TRACK);
			_tracks = new ObservableCollection<Track>(_search.Tracks);
		}

	}
}

