﻿using System;
using MonoTouch.UIKit;
using OpenPlaylistApp.Models;
using MonoTouch.Security;
using MonoTouch.Foundation;

namespace OpenPlaylistApp.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var uid = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            App.User.Id = uid;;

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
           
        }
    }
}
