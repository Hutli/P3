using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace OpenPlaylistApp
{
    public class HomePage : MasterDetailPage
    {
        private MasterView master;
        public HomeViewModel ViewModel { get { return BindingContext as HomeViewModel; } }

        public Action GoToPlaylist { get; set; }
        public HomePage()
        {
            Title = "Home";
            BindingContext = new HomeViewModel();

            var playlist = new PlaylistView();
            var searchTabs = new SearchTabView();
            var venueTab = new VenueViewTab();

            var playlistNav = new NavigationPage(playlist) { Title = playlist.Title };
            var searchNav = new NavigationPage(searchTabs) { Title = searchTabs.Title };
            var venueNav = new NavigationPage(venueTab) { Title = venueTab.Title };

            ViewModel.Pages.Add(playlistNav);
            ViewModel.Pages.Add(searchNav);
            ViewModel.Pages.Add(venueNav);

            Detail = playlistNav;
            IsPresented = false;

            GoToPlaylist = () =>
            {
                master.SelectedPage = playlistNav;
                IsPresented = false;

            };

            PlaylistViewModel.Home = this;
            Master = master = new MasterView(ViewModel);
            master.PageSelection = (x) =>
            {

                Detail = x;
                Detail.Title = x.Title;
                IsPresented = false;

            };
        }

        public class MasterView : ContentPage
        {
            public Page SelectedPage
            {
                get { return (Page)listView.SelectedItem; }
                set
                {
                    foreach (var item in viewModel.Pages)
                    {
                        if (item.Title == value.Title)
                        {
                            listView.SelectedItem = item;
                            PageSelection(item);
                        }
                    }
                }
            }

            private ListView listView;

            private HomeViewModel viewModel;
            public Action<Page> PageSelection { get; set; }
            public MasterView(HomeViewModel model)
            {
                viewModel = model;
                Title = "Menu";
                var stack = new StackLayout() { Spacing = 0 };
                this.BackgroundColor = Color.Silver;
                var label = new ContentView
                {
                    Padding = new Thickness(10, 36, 0, 5),
                    BackgroundColor = Color.Transparent,
                    Content = new Label
                    {
                        Text = "MENU",
                        Font = Font.SystemFontOfSize(NamedSize.Medium)
                    }
                };

                stack.Children.Add(label);

                listView = new ListView();

                var cell = new DataTemplate(typeof(ImageCell));
                cell.SetBinding(TextCell.TextProperty, "Title");
                cell.SetValue(TextCell.TextColorProperty, Color.Black);
                cell.SetBinding(ImageCell.ImageSourceProperty, "Icon");

                listView.ItemTemplate = cell;

                listView.ItemsSource = viewModel.Pages;

                listView.ItemSelected += (sender, args) =>
                {
                    var menuItem = listView.SelectedItem;
                    if (PageSelection != null)
                    {
                        PageSelection((Page)menuItem);
                    }
                };

                foreach (var item in viewModel.Pages)
                {
                    listView.SelectedItem = item;
                    break;
                }

                stack.Children.Add(listView);

                Content = stack;
            }
        }

    }
}

