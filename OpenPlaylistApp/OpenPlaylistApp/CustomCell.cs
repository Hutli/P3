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
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, string>(p => p.VoteString, default(string));


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

        public string VoteString
        {
            get { return (string)GetValue(VoteProperty); }
            set { SetValue(VoteProperty, value); }
        }

        Image _image = new Image();
        Label _voteLabel = new Label();
        Label _textLabel = new Label();
        Label _detailLabel = new Label();

        RelativeLayout _layout = new RelativeLayout();

        protected override void OnBindingContextChanged()
        {
            if (default(string) != ImageString)
            {
                _image.Source = ImageString;
                _layout.Children.Add(_image, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => parent.Width/5));
            }
            if (default(string) != VoteString)
            {
                _voteLabel.Text = VoteString;
                _layout.Children.Add(_voteLabel, Constraint.Constant(_image.Width), Constraint.RelativeToParent((parent) => parent.Height / 2));
            }
            if (default(string) != TextString)
            {
                _textLabel.Text = TextString;
                _layout.Children.Add(_textLabel,  Constraint.RelativeToParent((parent) => parent.Width/3 ));
            }
            if (default(string) != DetailString)
            {
                _detailLabel.Text = DetailString;
                _layout.Children.Add(_detailLabel, Constraint.RelativeToParent((parent) => parent.Width / 3), Constraint.RelativeToParent((parent) => parent.Height/3));
            }

            //if ()
            //    _layout.Opacity = 50;
        }

        public CustomCell()
            : base()
        {
            _voteLabel.Text = "2";
            _layout.HeightRequest = 60f;
            _layout.HorizontalOptions = LayoutOptions.FillAndExpand;

            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //_layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            View = _layout;
        }


    }
}
