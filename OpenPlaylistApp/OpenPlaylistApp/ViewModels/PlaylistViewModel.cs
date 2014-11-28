//using Android.App;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenPlaylistApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.ViewModels
{
    public class PlaylistViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action LoadComplete;

        public ObservableCollection<Track> Results
        {
            get;
            set;
        }

        public PlaylistViewModel(Venue venue)
        {
            Results = new ObservableCollection<Track>();
            GetResults(venue);
        }

        private void UpdateResults(IEnumerable<Track> newData)
        {
            var tmpNewData = newData.ToList();
            var tmpResults = Results.ToList();

            foreach (Track t in tmpNewData)
            {
                if (!Results.Contains(t))
                    Results.Add(t);
            }

            foreach (Track t in tmpResults)
            {
                if (!newData.Contains(t))
                    Results.Remove(t);
            }
        }

        async public void GetResults(Venue venue)
        {
            Session session = Session.Instance();
            ObservableCollection<Track> returnValue = new ObservableCollection<Track>();
            try
            {
                var json = await session.GetPlaylist(venue);
                returnValue = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                UpdateResults(returnValue.ToList());

                LoadComplete();
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

