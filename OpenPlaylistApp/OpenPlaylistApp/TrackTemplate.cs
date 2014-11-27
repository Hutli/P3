using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class TrackTemplate : DataTemplate
    {
        public TrackTemplate():base(typeof(ImageCell)){
            this.SetBinding(TextCell.TextProperty, "Name");
            this.SetBinding(TextCell.DetailProperty, "Album.Artists[0].Name");
            //this.SetBinding(TextCell.IsEnabledProperty, "IsFiltered");
            //this.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[0].URL");
        }
    }
}
