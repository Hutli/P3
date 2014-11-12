using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class VenueView : ContentView
    {

        public VenueView()
        {
            var list = new ListView();
            list.ItemTemplate = new VenueTemplate();
            list.ItemsSource = App.venues;
            Content = list;
        }
    }
}
