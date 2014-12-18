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
#if WINDOWS_PHONE
            label.SetBinding(Label.TextProperty, new Binding("AverageVolume", BindingMode.TwoWay));
            slider.SetBinding(Slider.ValueProperty, new Binding("SelectedVolume", BindingMode.TwoWay));
            #else
            label.SetBinding(Label.TextProperty, new Binding("AverageVolume", BindingMode.OneWay));
            slider.SetBinding(Slider.ValueProperty, new Binding("SelectedVolume", BindingMode.OneWayToSource));
#endif
            layout.Children.Add(label);
            layout.Children.Add(slider);
            Content = layout;
        }
    }
}