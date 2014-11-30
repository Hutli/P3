using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a venue
    class VenueTemplate : DataTemplate
    {
        public VenueTemplate():base(typeof(CustomCell)){
            this.SetBinding(CustomCell.TextProperty, "Name");
            this.SetBinding(CustomCell.DetailProperty, "IP");
            #if !WINDOWS_PHONE
                this.SetBinding(CustomCell.ImageSourceProperty, "IconUrl");
            #endif
        }
    }
}
