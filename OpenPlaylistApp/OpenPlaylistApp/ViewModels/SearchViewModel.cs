using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using OpenPlaylistApp.Models;
using WebAPI;

namespace OpenPlaylistApp.ViewModels {
    public class SearchViewModel : BaseViewModel {
        private ObservableCollection<Track> _results = new ObservableCollection<Track>();
        private Track _selectedItem;
        public int resultCount = 0;

        public ObservableCollection<Track> Results {
            get {return _results;}
            set {
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        /// <summary>
        ///     Gets or sets the selected feed item
        /// </summary>
        public Track SelectedItem {
            get {return _selectedItem;}
            set {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public async void GetResults(string searchStr) {
            if(IsBusy)
                return;

            IsBusy = true;

            try {
                var session = Session.Instance();
                Results.Clear();
                var json = await session.Search(App.User.Venue, searchStr);
                var returnValue =
                    (ObservableCollection<Track>)
                    JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                if(returnValue != null) {
                    foreach(var t in returnValue)
                        Results.Add(t);
                }
            } catch(Exception ex) {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
            IsBusy = false;
        }

        public async void GetResultsAndAppend(string searchStr, int offset) {
            if(App.Home.IsBusy)
                return;

            App.Home.IsBusy = true;

            try {
                var session = Session.Instance();
                var json = await session.Search(App.User.Venue, searchStr, offset);
                var returnValue =
                    (ObservableCollection<Track>)
                    JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                if(returnValue != null) {
                    foreach(var t in returnValue)
                        Results.Add(t);
                }
            } catch(Exception ex) {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
            App.Home.IsBusy = false;
        }
    }
}