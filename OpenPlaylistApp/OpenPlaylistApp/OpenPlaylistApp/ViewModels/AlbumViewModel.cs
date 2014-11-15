using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebAPILib;

namespace OpenPlaylistApp.ViewModels
{
    public class AlbumViewModel : BaseViewModel
    {
        public readonly Album Album;
        public ObservableCollection<Track> Tracks { get; set; }
        public AlbumViewModel(Album album)
        {
            Album = album;
            Tracks = new ObservableCollection<Track>();
        }

        public void GetTrack()
        {
            Task.Run(() =>
            {
                foreach (var item in Album.Tracks)
                {
                    Tracks.Add(item);
                }
                IsBusy = false;
            });
        }
    }
}
