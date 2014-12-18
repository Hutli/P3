using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using OpenPlaylistApp.Models;

namespace OpenPlaylistApp.ViewModels
{
    public class VenueViewModel : INotifyPropertyChanged
    {
        public VenueViewModel()
        {
            Results = new ObservableCollection<Venue>();
            GetResults();
        }

        public ObservableCollection<Venue> Results { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private async void GetResults()
        {
            var session = Session.Instance();
            try
            {
                var str = await session.GetVenues();
                var v = JArray.Parse(str);
                foreach(var item in v)
                    Results.Add(new Venue((string)item["name"], (string)item["detail"], (string)item["ip"], (string)item["iconUrl"]));
            } catch(Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}