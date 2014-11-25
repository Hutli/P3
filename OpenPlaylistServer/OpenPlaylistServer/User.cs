using System.ComponentModel;

namespace OpenPlaylistServer {
    public class User : INotifyPropertyChanged
    {
        private PlaylistTrack _vote;
        public string Id { get; private set; }

        public PlaylistTrack Vote
        {
            get { return _vote; }
            set
            {
                _vote = value;
                OnPropertyChanged("Vote");
            }
        }

        public User(string id) {
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        } 
    }
}
