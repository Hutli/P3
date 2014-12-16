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
            #if WINDOWS_PHONE
            image = new Xamarin.Forms.Image { Source = ImageSource.FromFile("Resources/checkin.png")};
#else
            image = new Xamarin.Forms.Image { Source = ImageSource.FromFile("checkin") };
            #endif
            image.Aspect = Aspect.AspectFit;
            Content = image;
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}
