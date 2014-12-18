using System;
using System.ComponentModel;
using System.Threading.Tasks;
using OpenPlaylistApp.Models;
using Xamarin.Forms;

namespace OpenPlaylistApp.ViewModels
{
    public class VolumeViewModel : INotifyPropertyChanged
    {
        private string _averageVolume;
        private int _selectedVolume;

        public VolumeViewModel()
        {
            SelectedVolume = 50;
            AverageVolume = "Not voted on volume yet";
            Task.Run(async () =>
                           {
                               while(true)
                               {
                                   await Task.Delay(TimeSpan.FromSeconds(3)); // update from server every second
                                   if(App.User != null && App.User.Venue != null)
                                   {
                                       Device.BeginInvokeOnMainThread((() =>
                                                                       {
                                                                           SetVolume(SelectedVolume, false);
                                                                           // get average volume by just sending the selected vote again
                                                                       }));
                                   }
                               }
                           });
        }

        public string AverageVolume
        {
            get { return _averageVolume; }
            set
            {
                _averageVolume = value;
                OnPropertyChanged("AverageVolume");
            }
        }

        public int SelectedVolume
        {
            get { return _selectedVolume; }
            set
            {
                _selectedVolume = value;
                OnPropertyChanged("SelectedVolume");
                SetVolume(value, true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SetVolume(int newValue, bool loadingIndicator)
        {
            /*
			Insights.Track("Send volume to server called", new Dictionary<string, string>{
				{"volumeValue", newValue.ToString()}
			});
			var timer = Insights.TrackTime ("SetVolume VoluemViewModel");
			timer.Start ();
			*/
            var progress = Convert.ToInt32(newValue);

            if(App.User == null)
                return;
            var Uri = Session.MakeUri("volume/" + progress + "/" + App.User.Id);
            if(Uri == null)
                return;
            var res = await Session.MakeRequest(Uri, "Volume error", "Could not set volume", new TimeSpan(0, 0, 7), loadingIndicator);
            //timer.Stop ();
            if(res == null)
                return;

            var average = -1;
            int.TryParse(res, out average);

            if(average != -1)
                AverageVolume = String.Format("Average volume: {0}%", average);
        }
    }
}