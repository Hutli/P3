using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    class CustomCell : ViewCell
    {
        //public static readonly BindableProperty SelectedProperty = BindableProperty.Create<CustomCell, bool>(p => p.SelectedBool, default(bool));
        public static readonly BindableProperty GrayoutProperty = BindableProperty.Create<CustomCell, bool>(p => p.GrayoutBool, default(bool));
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create<CustomCell, string>(p => p.ImageString, default(string));
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomCell, string>(p => p.TextString, default(string));
        public static readonly BindableProperty DetailProperty = BindableProperty.Create<CustomCell, string>(p => p.DetailString, default(string));
        public static readonly BindableProperty VoteProperty = BindableProperty.Create<CustomCell, int>(p => p.VoteString, default(int));

        //public bool SelectedBool
        //{
        //    get { return (bool)GetValue(SelectedProperty); }
        //    set { SetValue(SelectedProperty, value); }
        //}

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
            get
            {
                return (int) GetValue(VoteProperty); }
            set { SetValue(VoteProperty, value); }
        }

        Image _image = new Image();
        Label _voteLabel = new Label();
        Label _textLabel = new Label();
        Label _detailLabel = new Label();
        RelativeLayout _layout = new RelativeLayout();

        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    if (SelectedBool)
        //        Mark();
        //    else
        //        UnMark();
        //}

        //public void Mark()
        //{
        //    _layout.BackgroundColor = Color.Green;
        //}

        //public void UnMark()
        //{
        //    _layout.BackgroundColor = Color.Transparent;
        //}
        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    //if (propertyName == "SelectedBool")
        //    //{
        //        if (SelectedBool)
        //        {
        //            Mark();
        //        }
        //        else
        //        {
        //            UnMark();
        //        }
        //    //}
        //}
        protected override void OnBindingContextChanged()
        {
            if (default(string) != ImageString)
            {
                _image.Source = ImageString;
                _layout.Children.Add(_image, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((Parent) => Parent.Height), Constraint.RelativeToParent((Parent) => Parent.Height));
            }
            if (default(int) != VoteString)
            {
                _voteLabel.Text = String.Format("{0}", VoteString);
                _voteLabel.Font = Font.BoldSystemFontOfSize(40);
                var voteTextLabel = new Label()
                {
                    Text = "votes"
                };
                _layout.Children.Add(voteTextLabel, Constraint.RelativeToParent(parent => parent.Width - 100f), Constraint.RelativeToParent((parent) => parent.Height - 50f));
                
                _layout.Children.Add(_voteLabel, Constraint.RelativeToParent((parent) => parent.Width - 50f), Constraint.RelativeToParent((parent) => (parent.Height / 2) - (50f / 2)));
            }
            if (default(string) != TextString)
            {
                _textLabel.Text = TextString;
                _textLabel.Font = Font.BoldSystemFontOfSize(NamedSize.Large);
                _layout.Children.Add(_textLabel, Constraint.RelativeToParent((parent) => parent.Height + 10));
            }
            if (default(string) != DetailString)
            {
                _detailLabel.Text = DetailString;
                _detailLabel.Font = Font.SystemFontOfSize(NamedSize.Medium);
                _layout.Children.Add(_detailLabel, Constraint.RelativeToParent((parent) => parent.Height + 10), Constraint.RelativeToParent((parent) => parent.Height / 3));
            }

            if (GrayoutBool)
            {
                _layout.BackgroundColor = new Color(128, 128, 128);
                _layout.Opacity = 50;
            }
        }

        public CustomCell()
            : base()
        {
            _layout.HeightRequest = 100f;
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
