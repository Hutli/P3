using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class TrackTemplate : DataTemplate
    {
        public TrackTemplate():base(typeof(ImageCell)){
            this.SetBinding(ImageCell.TextProperty, "Name");
            this.SetBinding(ImageCell.DetailProperty, "Album.Artists[0].Name");
            this.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[0].URL");
        }
    }
}
