using System;
using System.Collections.Generic;
using System.Text;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    public class VolumeView : ContentView
    {
        public VolumeView()
        {
            BindingContext = new VolumeViewModel();
            var layout = new StackLayout();
            var slider = new Slider(0, 100, 50);
            var label = new Label();
            label.SetBinding(Label.TextProperty, new Binding("AverageVolume", BindingMode.OneWay));
            slider.SetBinding(Slider.ValueProperty, new Binding("SelectedVolume", BindingMode.OneWayToSource));
            layout.Children.Add(label);
            layout.Children.Add(slider);
            Content = layout;
        }
    }
}
