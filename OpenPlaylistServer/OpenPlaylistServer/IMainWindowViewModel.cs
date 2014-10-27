using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace OpenPlaylistServer
{
    public interface IMainWindowViewModel
    {
        ReadOnlyObservableCollection<PlaylistTrack> Tracks { get; }

        void TrackEnded();

        PlaylistTrack NextTrack();

        void PlayButonClicked();
    }
}
