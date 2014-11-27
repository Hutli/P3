using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using WebAPI;

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

        public CustomCell()
            : base()
        {
            Xamarin.Forms.Image _image = new Xamarin.Forms.Image { Source = ImageString };
            Label _voteLabel = new Label { Text = VoteString };
            Label _textLabel = new Label { Text = TextString };
            Label _detailLabel = new Label { Text = DetailString };
            
            StackLayout _layout = new StackLayout{ Orientation = StackOrientation.Horizontal };
            _layout.HeightRequest = 100;

            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //_layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //_layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            //_layout.Children.Add(_image, 0, 0, 0, 1);
            //_layout.Children.Add(_textLabel, 1, 1, 0, 0);
            //_layout.Children.Add(_detailLabel, 1, 1, 1, 1);
            //_layout.Children.Add(_voteLabel, 2, 2, 0, 1);

            _layout.Children.Add(_image);
            _layout.Children.Add(_textLabel);
            _layout.Children.Add(_detailLabel);
            _layout.Children.Add(_voteLabel);

            //if ()
            //    _layout.Opacity = 50;
            
            View = _layout;
        }


    }
}
