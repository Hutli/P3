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
            label.Text = "Volume: 0-100%";
            slider.ValueChanged += SetVolume;

            layout.Children.Add(label);
            layout.Children.Add(slider);
            Content = layout;
        }

        async void SetVolume(object sender, ValueChangedEventArgs e)
        {
            Session session = Session.Instance();
            await session.SetVolume(App.User.Venue, Convert.ToInt32(e.NewValue), App.User);
        }

    }
}
