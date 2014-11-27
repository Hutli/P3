using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class TrackTemplate : DataTemplate
    {
        public TrackTemplate():base(typeof(ImageCell)){
            //this.SetBinding(CustomCell.GrayoutProperty, "IsFiltered");
            this.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[0].URL");
            this.SetBinding(ImageCell.TextProperty, "Name");
            this.SetBinding(ImageCell.DetailProperty, "Album.Artist.FirstOrDefault().Name");
            //this.SetBinding(CustomCell.VoteProperty, "TotalScore");
        }
    }
}
