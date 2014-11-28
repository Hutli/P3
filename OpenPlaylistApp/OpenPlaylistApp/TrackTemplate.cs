using System.Collections;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    class TrackTemplate : DataTemplate
    {
        public TrackTemplate():base(typeof(CustomCell)){
            this.SetBinding(CustomCell.SelectedProperty, "IsSelected");
            this.SetBinding(CustomCell.GrayoutProperty, "IsFiltered");
            this.SetBinding(CustomCell.ImageSourceProperty, "Album.Images[0].URL");
            this.SetBinding(CustomCell.TextProperty, "Name");
            this.SetBinding(CustomCell.DetailProperty, "Album.Artist[0].Name");
            this.SetBinding(CustomCell.VoteProperty, "TotalScore");
        }
    }
}
