using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
	public partial class VolumeView : ContentView
	{
		public VolumeView ()
		{
            BindingContext = new VolumeViewModel();
			InitializeComponent ();
		}
	}
}
