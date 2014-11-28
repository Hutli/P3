using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class CustomCell : ViewCell
    {
        public static readonly BindableProperty GrayoutProperty = BindableProperty.Create<CustomCell, bool>(p => p.GrayoutBool, default(bool));
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create<CustomCell, string>(p => p.ImageString, default(string));
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomCell, string>(p => p.TextString, default(string));
        public static readonly BindableProperty DetailProperty = BindableProperty.Create<CustomCell, string>(p => p.DetailString, default(string));
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, int>(p => p.VoteString, default(int));

        public bool GrayoutBool
        {
            get { return (bool)GetValue(GrayoutProperty); }
            set { SetValue(GrayoutProperty, value); }
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
            _image.Source = ImageString;
            _voteLabel.Text = String.Format("{0}", VoteString);
            _textLabel.Text = TextString;
            _detailLabel.Text = DetailString;
        }

        public CustomCell()
            : base()
        {
            _layout.HeightRequest = 100f;
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
