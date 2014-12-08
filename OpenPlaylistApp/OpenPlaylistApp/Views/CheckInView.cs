using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class CheckInView : ContentView
    {
        private Image image;

        public CheckInView()
        {
            System.Uri uri = new Uri("https://www.dropbox.com/s/hldn5ddxr0zsdy4/checkin.png");
            //image.Source = ImageSource.(uri);

            image = new Xamarin.Forms.Image { Source = ImageSource.FromFile("checkin") }; ;
            image.Aspect = Aspect.AspectFit;
            Content = image;
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}
