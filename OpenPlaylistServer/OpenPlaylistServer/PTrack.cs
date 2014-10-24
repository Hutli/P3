using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyDotNet;

namespace OpenPlaylistServer
{
    public class PlaylistTrack : Track
    {
        private int _pScore = 0;
        private int _tScore = 0;

        public int TScore
        {
            get
            {
                return _tScore;
            }
            set
            {
                _tScore = value;
            }
        }

        public void UpdatePScore(List<User> users)
        {
            _pScore += users.Count;
        }

        public void ResetPScore()
        {
            _pScore = 0;
        }

        public int PScore
        {
            get
            {
                return _pScore;
            }
        }

        public PlaylistTrack(string trackUri)
            : base(trackUri)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
