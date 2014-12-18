using System;
using System.ComponentModel;
using Newtonsoft.Json;
using OpenPlaylistApp.Models;
using WebAPI;

namespace OpenPlaylistApp.ViewModels {
    public class NowPlayingViewModel : INotifyPropertyChanged {
        public NowPlayingViewModel(Venue venue) {
            GetResult(venue);
        }

        public Track Result {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action LoadComplete;

        public async void GetResult(Venue venue) {
            var session = Session.Instance();
            try {
                var json = await session.GetNowPlaying(venue);

                if(json == "Nothing currently playing")
                    return;
                App.Home.IsBusy = false;
                Result = (Track)JsonConvert.DeserializeObject(json, typeof(Track));
                if(App.User.Vote != null && Result.Equals(App.User.Vote))
                    App.User.Vote = null;
                LoadComplete();
            } catch(Exception) {
                //App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}