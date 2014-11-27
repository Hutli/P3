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
    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Track> Results
        {
            get;
            set;
        }

        public SearchViewModel(string searchStr)
        {
            Results = new ObservableCollection<Track>();
            GetResults(searchStr);
        }


        async public void GetResults(string searchStr){
            try
            {
            Session session = Session.Instance();
            ObservableCollection<Track> returnValue = new ObservableCollection<Track>();
            
            var json = await session.Search(App.User.Venue, searchStr);
            returnValue = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));
            Results.Clear();
            if (returnValue != null)
            {
                foreach (Track t in returnValue)
                {
                    Results.Add(t);
                }
            }
                
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

