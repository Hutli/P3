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
