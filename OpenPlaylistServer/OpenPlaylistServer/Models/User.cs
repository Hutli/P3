using System.ComponentModel;
using WebAPI;

namespace OpenPlaylistServer.Models
{
    public class User : INotifyPropertyChanged
    {
        private float _volume;
        private Vote _vote;

        public User(string id)
        {
            Id = id;
            Volume = 0.5f;
        }

        public string Id { get; private set; }

        public float Volume
        {
            get { return _volume; }
            set
            {
                if(!(value >= 0) || !(value <= 1))
                    return;
                _volume = value;
                OnPropertyChanged("Volume");
            }
        }

        public bool CheckedIn { get; set; }

        public Vote Vote
        {
            get { return _vote; }
            set
            {
                _vote = value;
                // TODO: Den her linje crasher programmet hvis en anden sang er ved at blive spillet... mystisk
                OnPropertyChanged("Vote");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string pName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }
}