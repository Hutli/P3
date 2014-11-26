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
            Result = new Track("", "", 0, false, 0, "", "", new Album("", "", "", new List<WebAPI.Image>(), new List<Artist>()));
            Result.Album.Artists.Add(new Artist("","",new List<string>()));

            GetResult(venue);
        }

        async void GetResult(Venue venue)
        {
            Session session = Session.Instance();
            try
            {
                var json = await session.GetNowPlaying(venue);
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

