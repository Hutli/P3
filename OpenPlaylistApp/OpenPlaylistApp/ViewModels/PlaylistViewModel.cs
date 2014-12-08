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
    public class PlaylistViewModel : BaseViewModel
    {
        public event Action LoadComplete;

        private ObservableCollection<Track> _results = new ObservableCollection<Track>();

        public ObservableCollection<Track> Results
        {
            get { return _results; }
            set { _results = value; OnPropertyChanged("Results"); }
        }

        private Track _selectedItem;
        /// <summary>
        /// Gets or sets the selected feed item
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

        public PlaylistViewModel() {
        }

        private void UpdateResults(ObservableCollection<Track> newData)
        {
            for (int i = 0; i < newData.Count; i++)
            {
                if (i < Results.Count)
                {
                    if (!newData[i].Equals(Results[i]))
                    {
                        if (SelectedItem != null && newData[i].Id.Equals(SelectedItem.Id))
                            newData[i].IsSelected = true;
                        Results[i] = newData[i];
                    }
                }
                else
                {
                    Results.Add(newData[i]);
                }
            }

            //var toAdd = tmpNewData.FindAll(p => !tmpResults.Contains(p));
            //var toRemove = tmpResults.FindAll(p => !tmpNewData.Contains(p));

            //foreach (Track t in Results)
            //{
            //    var tmpTrack = tmpNewData.FirstOrDefault(p => p.Equals(t));
            //    if (tmpTrack != null && tmpTrack.TotalScore != t.TotalScore)
            //    {
            //        toRemove.Add(t);
            //        toAdd.Add(tmpTrack);
            //    }
            //}

            //foreach (Track t in toRemove)
            //    Results.Remove(t);

            //foreach (Track t in toAdd)
            //    Results.Add(t);

            //if (Results.Contains(SelectedItem))
            //{
            //    Track tmpTrack = Results.First(p => p.Equals(SelectedItem));
            //    if (!tmpTrack.IsSelected)
            //    {
            //        SelectedItem = tmpTrack;
            //        tmpTrack.IsSelected = true;
            //        Results.Remove(tmpTrack);
            //        Results.Add(tmpTrack);
            //    }
            //}

            //tmpResults = new List<Track>(Results);
            //tmpResults.Sort((x, y) => x.TotalScore.CompareTo(y.TotalScore));
            //Results = new ObservableCollection<Track>(tmpResults);

            //ObservableCollection<Track> tmpListResults = (ObservableCollection<Track>)Results;
            //bool evalBool = false;
            //if (tmpListResults.Count == Results.Count)
            //{
            //    for (int i = 0; i < tmpListResults.Count; i++)
            //    {
            //        if (!tmpListResults[i].Equals(Results[i]))
            //        {
            //            evalBool = true;
            //            break;
            //        }
            //    }
            //}
            //else { evalBool = true; }
            //if (evalBool) { Results = tmpListResults; }

            this.OnPropertyChanged("TotalScore");
        }

        async public void GetResults(Venue venue)
        {
            Session session = Session.Instance();
            ObservableCollection<Track> returnValue = new ObservableCollection<Track>();
            try
            {
                var json = await session.GetPlaylist(venue);
                returnValue = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                UpdateResults(returnValue);

                LoadComplete();
            }
            catch (Exception ex)
            {
                //App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

