using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class VenueTemplate : DataTemplate
    {
        public VenueTemplate():base(typeof(CustomCell)){
            this.SetBinding(CustomCell.TextProperty, "Name");
            this.SetBinding(CustomCell.DetailProperty, "IP");
            this.SetBinding(CustomCell.ImageSourceProperty, "IconUrl");
        }
    }
}
