using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
using Microsoft.Phone.Info;
using OpenPlaylistApp.Models;
using System.Runtime.InteropServices;


namespace OpenPlaylistApp.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            Content = OpenPlaylistApp.App.GetMainPage().ConvertPageToUIElement(this);

            object uniqueId;
            var hexString = string.Empty;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                hexString = BitConverter.ToString((byte[])uniqueId);
            
            OpenPlaylistApp.App.User.Id = hexString;
        }
    }
}
