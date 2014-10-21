using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace OpenPlaylistApp
{

    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<Page> Pages { get; set; }

        public HomeViewModel()
        {
            Pages = new ObservableCollection<Page>();
        }
    }
}
