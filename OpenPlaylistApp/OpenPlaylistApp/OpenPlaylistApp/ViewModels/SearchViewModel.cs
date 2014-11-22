using System.Threading.Tasks;
using WebAPI;

namespace OpenPlaylistApp.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(string str)
        {
            HomePage.Search.Clear();
            ExecuteloadSongsCommand(str);
        }

        public async void ExecuteloadSongsCommand(string searchStr)
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                /*Search search = new Search(searchStr);
                foreach (Track item in search.Tracks)
                    App.Search.Add(item);
                IsBusy = false;*/
            });
        }

    }
}

