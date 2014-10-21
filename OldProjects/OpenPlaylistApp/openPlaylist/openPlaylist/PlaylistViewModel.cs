using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WebApiLib;

namespace openPlaylist
{
    public static class PlaylistViewModel
    {
        public static ObservableCollection<Track> Tracks { get; set; }

        public static event Action TrackSelected;

        public static HomePage Home { get; set; }

        private static Track track;
        public static Track vote
        {
            get { return track; }
            set
            {
                track = value; Tracks.Clear();
                Tracks.Add(track);
                TrackSelected();
            }
        }

        static PlaylistViewModel()
        {
            Tracks = new ObservableCollection<Track>();
        }
    }
}
