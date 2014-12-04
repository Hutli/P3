using System;
using System.ComponentModel;
using WebAPI;

namespace OpenPlaylistServer.Models
{
    public class Restriction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Func<Track,bool> _predicate;

        private TimeSpan _timeStart;
        private TimeSpan _timeEnd; // Maybe duration instead

        public string Time
        {
            get { return _timeStart.ToString("G") + " - " + _timeStart.ToString("G"); }
        }

        public Restriction(Func<Track, bool> predicate, TimeSpan timeStart, TimeSpan timeEnd)
        {
            _predicate = predicate;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
        }

        public Func<Track, bool> Predicate
        {
            get { return _predicate; }
        }

        //ToStringPredicate

        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        } 
    }
}
