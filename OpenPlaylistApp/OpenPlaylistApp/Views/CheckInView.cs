using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class CheckInView : ContentView
    {
        private Label label = new Label { Text = "Please check in at a venue" , HorizontalOptions = LayoutOptions.CenterAndExpand };
        private Image image = new Image { Source = ImageSource.FromResource("checkin"), HorizontalOptions = LayoutOptions.CenterAndExpand };
        private StackLayout layout = new StackLayout();

        public CheckInView()
        {
            layout.Children.Add(label);
            layout.Children.Add(image);
            Content = layout;
        }
    }
}
