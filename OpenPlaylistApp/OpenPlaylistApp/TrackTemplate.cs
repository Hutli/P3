using System.Collections;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class TrackTemplate : DataTemplate
    {
        public TrackTemplate():base(typeof(ImageCell)){
            //this.SetBinding(CustomCell.GrayoutProperty, "IsFiltered");
            this.SetBinding(ImageCell.ImageSourceProperty, "Album.Images[1].URL");
            this.SetBinding(ImageCell.TextProperty, "Name");
            this.SetBinding(ImageCell.DetailProperty, "Album.Artist[0].Name");
            //this.SetBinding(CustomCell.VoteProperty, "TotalScore");
        }
    }
}
