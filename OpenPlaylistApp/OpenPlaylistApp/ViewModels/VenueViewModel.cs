using Newtonsoft.Json.Linq;
using OpenPlaylistApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OpenPlaylistApp.ViewModels
{
    public class VenueViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Venue> Results
        {
            get;
            set;
        }

        public VenueViewModel()
        {
            Results = new ObservableCollection<Venue>();
            GetResults();
        }

        async void GetResults()
        {
            Session session = Session.Instance();
            try
            {
                var str = await session.GetVenues();
                JArray v = JArray.Parse(str);
                foreach (var item in v)
                {
                    Results.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"], (string)item["iconUrl"]));
                }
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

