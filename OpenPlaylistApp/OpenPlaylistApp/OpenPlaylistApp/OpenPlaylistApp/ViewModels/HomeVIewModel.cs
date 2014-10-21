using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp2
{
    public class HomeVIewModel : BaseViewModel
    {
        public ObservableDictionary<string, Page> Pages { get; set; }

        public HomeVIewModel()
        {
            Pages = new ObservableDictionary<string, Page>();
        }
    }
}
