using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class VenueTemplate : DataTemplate
    {
        public VenueTemplate():base(typeof(ImageCell)){
            this.SetBinding(ImageCell.TextProperty, "Name");
            this.SetBinding(ImageCell.DetailProperty, "Detail");
            this.SetBinding(ImageCell.ImageSourceProperty, "IconUrl");
        }
    }
}
