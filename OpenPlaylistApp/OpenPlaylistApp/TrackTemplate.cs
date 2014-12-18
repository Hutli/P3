using Xamarin.Forms;

namespace OpenPlaylistApp
{
    //This is a template for how to diplay a track
    internal class TrackTemplate : DataTemplate
    {
        public TrackTemplate() : base(typeof(CustomCell))
        {
            this.SetBinding(CustomCell.SelectedProperty, "IsSelected");
            this.SetBinding(CustomCell.FilteredProperty, "IsFiltered");
            this.SetBinding(CustomCell.ImageSourceProperty, "Album.Images[2].Url");
            this.SetBinding(CustomCell.TextProperty, "Name");
            this.SetBinding(CustomCell.DetailProperty, "Album.ArtistsToString");
            this.SetBinding(CustomCell.VoteProperty, "TotalScore");
        }
    }
}