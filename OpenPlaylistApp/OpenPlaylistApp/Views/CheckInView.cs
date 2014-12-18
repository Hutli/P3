using Xamarin.Forms;

namespace OpenPlaylistApp.Views {
    internal class CheckInView : ContentView {
        private readonly Image image;

        public CheckInView() {
#if WINDOWS_PHONE
            image = new Xamarin.Forms.Image { Source = ImageSource.FromFile("Resources/checkin.png")};
#else
            image = new Image {
                Source = ImageSource.FromFile("checkin")
            };
#endif
            image.Aspect = Aspect.AspectFit;
            Content = image;
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}