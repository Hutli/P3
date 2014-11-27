using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class CustomCell : ViewCell
    {
        public static readonly BindableProperty GrayoutProperty = BindableProperty.Create<CustomCell, string>(p => p.GrayoutString, string.Empty);
        public static readonly BindableProperty ImageProperty = BindableProperty.Create<CustomCell, string>(p => p.ImageString, string.Empty);
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomCell, string>(p => p.TextString, string.Empty);
        public static readonly BindableProperty DetailProperty = BindableProperty.Create<CustomCell, string>(p => p.DetailString, string.Empty);
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, string>(p => p.VoteString, string.Empty);

        public string GrayoutString
        {
            get { return (string)GetValue(GrayoutProperty); }
            set { SetValue(GrayoutProperty, value); }
        }

        public string ImageString
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
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

        private Image _image = new Image();
        private Label _voteLabel = new Label();
        private Label _textLabel = new Label();
        private Label _detailLabel = new Label();
        private Grid _layout = new Grid();

        public CustomCell()
            : base()
        {
            _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            bool grayout = (bool)GetValue(GrayoutProperty);
            _image.Source = (string)GetValue(ImageProperty);
            _voteLabel.Text = (string)GetValue(VoteProperty);
            _textLabel.Text = (string)GetValue(TextProperty);
            _detailLabel.Text = (string)GetValue(DetailProperty);

            _layout.Children.Add(_image, 0, 0, 0, 1);
            _layout.Children.Add(_textLabel, 1, 1, 0, 0);
            _layout.Children.Add(_detailLabel, 1, 1, 1, 1);
            _layout.Children.Add(_voteLabel, 2, 2, 0, 1);

            if (grayout)
                _layout.Opacity = 50;

            View = _layout;
        }


    }
}
