using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TestApp2
{
	public class HomePage : MasterDetailPage
	{
        private MasterView master;
        public HomeVIewModel ViewModel { get { return BindingContext as HomeVIewModel; } }

        public Action GoToPlaylist { get; set; }
		public HomePage ()
		{
			Title = "Home";
            BindingContext = new HomeVIewModel();

            var playlist = new PlaylistView();

            var searchTabs = new SearchTabView();

            var PlaylistNav = new NavigationPage(playlist) { Title = playlist.Title };
            var SearchNav = new NavigationPage(searchTabs) { Title = searchTabs.Title };

            ViewModel.Pages.Add(PlaylistNav.Title, PlaylistNav);
            ViewModel.Pages.Add(SearchNav.Title, SearchNav);

            Detail = PlaylistNav;
            IsPresented = false;

            GoToPlaylist = () => {
                master.SelectedPage = PlaylistNav;
                IsPresented = false;
                
            };

            PlaylistVIewModel.Home = this;
            Master = master = new MasterView(ViewModel);
            master.PageSelection = (x) => {

                Detail = x;
                Detail.Title = x.Title;
                IsPresented = false;

            };
		}

        public class MasterView : ContentPage
        {
            public Page SelectedPage { get { return ((KeyValuePair<string, Page>)listView.SelectedItem).Value; }
                set
                {
                    foreach (var item in viewModel.Pages)
                    {
                        if(item.Value.Title == value.Title)
                        {
                            listView.SelectedItem = item;
                            PageSelection(item.Value);
                        }
                    }
                }
            }

            private ListView listView;

            private HomeVIewModel viewModel;
            public Action<Page> PageSelection { get; set; }
            public MasterView(HomeVIewModel model)
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
                cell.SetBinding(TextCell.TextProperty, "Key");
                cell.SetValue(TextCell.TextColorProperty, Color.Black);
                cell.SetBinding(ImageCell.ImageSourceProperty, "Value.Icon");

                listView.ItemTemplate = cell;

                listView.ItemsSource = viewModel.Pages;

                listView.ItemSelected += (sender, args) =>
                {
                    var menuItem = (KeyValuePair<string, Page>)listView.SelectedItem;
                    var item = menuItem.Value;
                    if(PageSelection != null)
                    {
                        PageSelection(item);
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

