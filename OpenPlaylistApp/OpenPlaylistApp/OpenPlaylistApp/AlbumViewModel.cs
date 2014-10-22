using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPILib;

namespace OpenPlaylistApp
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

        public void getTrack()
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
