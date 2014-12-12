using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WebAPI {
    public class Vote : INotifyPropertyChanged
    {
        private Track _track;

        public Vote(Track track)
        {
            Track = track;
            Time = DateTime.Now;
        }

        public DateTime Time { get; private set; }

        public Track Track { 
            get { return _track; }
            set
            {
                _track = value;
                OnPropertyChanged("Track");
            }
        }

        public static implicit operator Track(Vote vote)
        {
            return vote._track;
        }

        public static implicit operator Vote(Track t)
        {
            return new Vote(t);
        }

        public override string ToString()
        {
            return _track.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
