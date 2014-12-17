using System;
using System.ComponentModel;

namespace WebAPI
{
    public class Vote : INotifyPropertyChanged
    {
        private Track _track;

        private Vote(Track track)
        {
            Track = track;
            Time = DateTime.Now;
        }

        private DateTime Time { get; set; }

        public Track Track
        {
            get { return _track; }
            private set
            {
                _track = value;
                OnPropertyChanged("Track");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public static implicit operator Track(Vote vote) { return vote._track; }
        public static implicit operator Vote(Track t) { return new Vote(t); }
        public override string ToString() { return _track.ToString(); }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}