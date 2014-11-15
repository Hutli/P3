using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class VenueTemplate : DataTemplate
    {
        public VenueTemplate():base(typeof(ImageCell)){
            this.SetBinding(TextCell.TextProperty, "Name");
            this.SetBinding(TextCell.DetailProperty, "Detail");
            this.SetBinding(ImageCell.ImageSourceProperty, "IconUrl");
        }
    }
}
