using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPlaylistServer
{
    public interface IPlaybackService
    {
        void Play(PlaylistTrack track);
        void Stop();
    }
}
