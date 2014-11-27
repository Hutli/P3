using Newtonsoft.Json;
using OpenPlaylistApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.ViewModels
{
    public class NowPlayingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action LoadComplete;

        public Track Result
        {
            get;
            set;
        }

        public NowPlayingViewModel(Venue venue)
        {
            GetResult(venue);
        }

        async public void GetResult(Venue venue)
        {
            Session session = Session.Instance();
            try
            {
                var json = await session.GetNowPlaying(venue);
                if (json == "NaN") { return; }
                Result = (Track)JsonConvert.DeserializeObject(json, typeof(Track));
                LoadComplete();
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }
    }
}

