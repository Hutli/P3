﻿using System;
using OpenPlaylistApp.ViewModels;
using Xamarin.Forms;
using OpenPlaylistApp.Models;
using WebAPI;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace OpenPlaylistApp.Views
{
    public class SearchView : ContentView
    {
        SearchBar searchBar = new SearchBar();
        ListView listView = new ListView();
        StackLayout layout = new StackLayout();
        SearchViewModel searchViewModel;

        public SearchView()
        {
            Session session = Session.Instance();
            listView.ItemSelected += session.ItemSelected; //Vote
            searchBar.SearchButtonPressed += SearchButtonPressed;

            ActivityIndicator ac = new ActivityIndicator()
            {
                IsEnabled = false,
                IsVisible = false,
                IsRunning = false
            };
            ac.SetBinding(ActivityIndicator.IsRunningProperty, "isBusy");
            ac.SetBinding(ActivityIndicator.IsEnabledProperty, "isBusy");
            ac.SetBinding(ActivityIndicator.IsVisibleProperty, "isBusy");

            layout.Children.Add(searchBar);
            layout.Children.Add(ac);
            layout.Children.Add(listView);
            Content = layout;

            //search.TextChanged += search_SearchButtonPressed; search as you type, maybe introduce delay
        }

        async void SearchButtonPressed(object sender, EventArgs e)
        {
            searchViewModel = new SearchViewModel(((SearchBar)sender).Text);
            searchViewModel.isBusy = true;
            listView.ItemsSource = searchViewModel.Results;
            listView.ItemTemplate = new TrackTemplate();
            searchViewModel.isBusy = false;
        }
    }
}