using Xamarin.Forms;

namespace OpenPlaylistApp {
    //This is a template for how to diplay a venue
    internal class VenueTemplate : DataTemplate {
        public VenueTemplate() : base(typeof(ImageCell)) {
            this.SetBinding(TextCell.TextProperty, "Name");
            //this.SetBinding(ImageCell.DetailProperty, "IP");
#if !WINDOWS_PHONE
            this.SetBinding(ImageCell.ImageSourceProperty, "IconUrl");
#endif
        }
    }
}