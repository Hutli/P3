using OpenPlaylistApp.Models;
using System;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class VolumeView : ContentView
    {
        private Label label = new Label();
        private StackLayout layout = new StackLayout();
        private Slider slider = new Slider(0, 100, 50);

        public VolumeView()
        {
            label.Text = "You can influence the volume below";
            slider.ValueChanged += SetVolume;

            layout.Children.Add(label);
            layout.Children.Add(slider);
            Content = layout;
        }

        async void SetVolume(object sender, ValueChangedEventArgs e)
        {
            var session = Session.Instance();
            var progress = Convert.ToInt32(e.NewValue);
            var average = await session.SetVolume(App.User.Venue, progress, App.User);
            label.Text = String.Format("Average volume: {0}%", average);
        }
    }
}
