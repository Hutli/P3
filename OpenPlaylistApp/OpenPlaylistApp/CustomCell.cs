using Xamarin.Forms;

namespace OpenPlaylistApp
{
    internal class CustomCell : ViewCell
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create<CustomCell, string>(p => p.ImageString, default(string));
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomCell, string>(p => p.TextString, default(string));
        public static readonly BindableProperty DetailProperty = BindableProperty.Create<CustomCell, string>(p => p.DetailString, default(string));
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, int>(p => p.VoteString, default(int));
        public static readonly BindableProperty FilteredProperty = BindableProperty.Create<CustomCell, bool>(p => p.FilteredBool, default(bool));
        public static readonly BindableProperty SelectedProperty = BindableProperty.Create<CustomCell, bool>(p => p.SelectedBool, default(bool));
        private ScrollView _test = new ScrollView();
        private readonly Label _detailLabel = new Label();
        private readonly Image _image = new Image();
        private readonly Grid _layout = new Grid();
        private readonly Label _textLabel = new Label();
        private readonly Label _voteLabel = new Label();

        public CustomCell()
        {
            var imageAspect = App.User.ScreenHeight / 8;

            _layout.RowDefinitions.Add(new RowDefinition());
            // { Height = new GridLength(imageAspect, GridUnitType.Star) });
            _layout.RowDefinitions.Add(new RowDefinition());
            _layout.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(imageAspect)});
            //, GridUnitType.Star) });
            _layout.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            _layout.ColumnDefinitions.Add(new ColumnDefinition());
            _image.HeightRequest = imageAspect;
            _layout.HeightRequest = imageAspect;

            _image.Aspect = Aspect.AspectFill;
            _layout.Children.Add(_image, 0, 0);
            Grid.SetRowSpan(_image, 2);

            _textLabel.LineBreakMode = LineBreakMode.TailTruncation;
            _textLabel.VerticalOptions = LayoutOptions.End;
            _textLabel.Font = Font.SystemFontOfSize(NamedSize.Medium);
            _layout.Children.Add(_textLabel, 1, 0);

            _detailLabel.LineBreakMode = LineBreakMode.TailTruncation;
            _detailLabel.VerticalOptions = LayoutOptions.Start;
            _detailLabel.Font = Font.SystemFontOfSize(NamedSize.Small);
            _layout.Children.Add(_detailLabel, 1, 1);

            _voteLabel.VerticalOptions = LayoutOptions.Center;
            _voteLabel.HorizontalOptions = LayoutOptions.Center;
            _voteLabel.WidthRequest = 36f;
            _layout.Children.Add(_voteLabel, 2, 0);
#if WINDOWS_PHONE
            _voteLabel.Font = Font.BoldSystemFontOfSize(20f);
#else
            _voteLabel.Font = Font.SystemFontOfSize(20f, FontAttributes.Bold);
#endif
            Grid.SetRowSpan(_voteLabel, 2);

            View = _layout;
        }

        public bool SelectedBool
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public bool FilteredBool
        {
            get { return (bool)GetValue(FilteredProperty); }
            set { SetValue(FilteredProperty, value); }
        }

        public string ImageString
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string TextString
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DetailString
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public int VoteString
        {
            get { return (int)GetValue(VoteProperty); }
            set { SetValue(VoteProperty, value); }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if(SelectedBool)
#if WINDOWS_PHONE
                    _layout.BackgroundColor = Color.Accent;
                #else
                _layout.BackgroundColor = Color.Green;
#endif
            else
                _layout.BackgroundColor = Color.Transparent;
        }

        protected override void OnBindingContextChanged()
        {
            if(_image.Source == null || !_image.Source.Equals(ImageString))
                _image.Source = ImageString;
            if(_textLabel.Text == null || !_textLabel.Equals(TextString))
                _textLabel.Text = TextString;
            if(_detailLabel.Text == null || !_detailLabel.Equals(DetailString))
                _detailLabel.Text = DetailString;
            if(_textLabel.Height + _detailLabel.Height > 0)
                _layout.HeightRequest = _textLabel.Height + _detailLabel.Height;
            if(FilteredBool)
                _layout.Opacity = 0.30f;
            if(VoteString > 0 && (_voteLabel.Text == null || !_voteLabel.Equals(VoteString)))
                _voteLabel.Text = VoteString.ToString();
        }
    }
}