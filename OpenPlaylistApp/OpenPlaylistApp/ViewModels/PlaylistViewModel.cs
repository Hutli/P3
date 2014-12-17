//using Android.App;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using OpenPlaylistApp.Models;
using WebAPI;

namespace OpenPlaylistApp.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private ObservableCollection<Track> _results = new ObservableCollection<Track>();
        private Track _selectedItem;
        public PlaylistViewModel() { App.User.VoteChanged += VoteChanged; }

        public ObservableCollection<Track> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        /// <summary>
        ///     Gets or sets the selected feed item
        /// </summary>
        public Track SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public event Action LoadComplete;

        private void VoteChanged(Track inputTrack)
        {
            var oldSelected = Results.FirstOrDefault(p => p.IsSelected);
            var newSelected = Results.FirstOrDefault(p => p.Id.Equals(inputTrack.Id));
            if(oldSelected != null)
            {
                oldSelected.IsSelected = false;
                Results[Results.IndexOf(oldSelected)] = oldSelected;
            }
            if(newSelected != null)
            {
                newSelected.IsSelected = true;
                Results[Results.IndexOf(newSelected)] = newSelected;
            } else
            {
                inputTrack.IsSelected = true;
                Results.Add(inputTrack);
            }
        }

        private void UpdateResults(ObservableCollection<Track> newData)
        {
            if(App.User.Vote != null)
            {
                var selectedTrack = newData.FirstOrDefault(p => p.Id.Equals(App.User.Vote.Id));

                if(selectedTrack != null)
                    selectedTrack.IsSelected = true;
            }

            int i;
            for(i = 0; i < newData.Count; i++)
            {
                if(i < Results.Count)
                {
                    if(!newData[i].Equals(Results[i]) || newData[i].TotalScore != Results[i].TotalScore)
                        Results[i] = newData[i];
                } else
                    Results.Add(newData[i]);
            }
            while(i < Results.Count)
                Results.RemoveAt(i);

            OnPropertyChanged("TotalScore");
        }

        public async void GetResults(Venue venue)
        {
            var session = Session.Instance();
            var returnValue = new ObservableCollection<Track>();
            try
            {
                var json = await session.GetPlaylist(venue);
                returnValue = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                UpdateResults(returnValue);

                LoadComplete();
            } catch(Exception)
            {
            }
        }
    }
}