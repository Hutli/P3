using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using OpenPlaylistApp.Views;
using Xamarin.Forms;
using OpenPlaylistApp.Models;
using System.Threading.Tasks;

namespace OpenPlaylistApp.ViewModels
{
    public class VolumeViewModel : INotifyPropertyChanged
    {
		private string _averageVolume;
        private int _selectedVolume;

        public string AverageVolume {
			get 
			{
				return _averageVolume;
			}
			set
			{ 
				_averageVolume = value;
				OnPropertyChanged("AverageVolume");
			}
		}

        public VolumeViewModel()
        {
            SelectedVolume = 50;
            AverageVolume = "Not voted on volume yet";
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3)); // update from server every second
                    if (App.User != null && App.User.Venue != null)
                    {
                        Device.BeginInvokeOnMainThread((() =>
                        {
                            SetVolume(SelectedVolume, false); // get average volume by just sending the selected vote again
                        }));
                    }
                }
            });
        }

        public int SelectedVolume
        {
            get
            {
                return _selectedVolume;
            }
            set
            {
                _selectedVolume = value;
                OnPropertyChanged("SelectedVolume");
                SetVolume(value, true);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void SetVolume(int newValue, bool loadingIndicator)
		{
			var progress = Convert.ToInt32(newValue);

            if (App.User == null)
		    {
		        return;
		    }
			var uri = Session.MakeUri ("volume/" + progress + "/" + App.User.Id);
		    if (uri == null)
		    {
		        return;
		    }
			var res = await Session.MakeRequest(uri, "Volume error", "Could not set volume", new TimeSpan(0,0,3), loadingIndicator);

			var average = int.Parse(res);

            AverageVolume = String.Format("Average volume: {0}%", average);
		}
    }
}
