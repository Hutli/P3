using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class CustomCell : ViewCell
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create<CustomCell, string>(p => p.ImageString, default(string));
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomCell, string>(p => p.TextString, default(string));
        public static readonly BindableProperty DetailProperty = BindableProperty.Create<CustomCell, string>(p => p.DetailString, default(string));
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, int>(p => p.VoteString, default(int));
        public static readonly BindableProperty FilteredProperty = BindableProperty.Create<CustomCell, bool>(p => p.FilteredBool, default(bool));

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

        Image _image = new Image();
        Label _voteLabel = new Label();
        Label _textLabel = new Label();
        Label _detailLabel = new Label();

        RelativeLayout _layout = new RelativeLayout();

        protected override void OnBindingContextChanged()
        {
            if (_image.Source == null || !_image.Source.Equals(ImageString))
                _image.Source = ImageString;
            if (_voteLabel == null || !_voteLabel.Equals(VoteString))
                _voteLabel.Text = VoteString.ToString();
            if (_textLabel == null || !_textLabel.Equals(TextString))
                _textLabel.Text = TextString;
            if (_detailLabel == null || !_detailLabel.Equals(DetailString))
                _detailLabel.Text = DetailString;
            if (FilteredBool)
            {
                _layout.Opacity = 0.30f;
                //_layout.BackgroundColor = Color.Gray;
            }
        }

        public CustomCell()
            : base()
        {
            _layout.HeightRequest = App.User.ScreenHeight / 8;
            _layout.WidthRequest = App.User.ScreenWidth;
            _layout.HorizontalOptions = LayoutOptions.FillAndExpand;

            _layout.Children.Add(_image, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((Parent) => Parent.Height), Constraint.RelativeToParent((Parent) => Parent.Height));

            _voteLabel.Font = Font.BoldSystemFontOfSize(40);
            _layout.Children.Add(_voteLabel, Constraint.RelativeToParent((parent) => parent.Width - 50f), Constraint.RelativeToParent((parent) => (parent.Height / 2) - (50f / 2)));

            _textLabel.Font = Font.BoldSystemFontOfSize(NamedSize.Large);
            _layout.Children.Add(_textLabel, Constraint.RelativeToParent((parent) => parent.Height + 10));

            _detailLabel.Font = Font.SystemFontOfSize(NamedSize.Medium);
            _layout.Children.Add(_detailLabel, Constraint.RelativeToParent((parent) => parent.Height + 10), Constraint.RelativeToParent((parent) => parent.Height / 3));

            View = _layout;
        }


    }
}
