using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class VolumeView : ContentView
    {
        public VolumeView()
        {
            Slider slider = new Slider(0,100,50);
            Content = slider;
        }

    }
}
