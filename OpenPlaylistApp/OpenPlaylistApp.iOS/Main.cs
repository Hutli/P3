using System;
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
            //var uid = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            //App.User.Id = uid;

            App.User.Id = UniqueID();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
           
        }

        public static string UniqueID()
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = NSBundle.MainBundle.BundleIdentifier;
            query.Account = "UniqueID";

            NSData uniqueId = SecKeyChain.QueryAsData(query);
            if (uniqueId == null)
            {
                query.ValueData = NSData.FromString(System.Guid.NewGuid().ToString());
                var err = SecKeyChain.Add(query);
                if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
                    throw new Exception("Cannot store Unique ID");

                return query.ValueData.ToString();
            }
            else
            {
                return uniqueId.ToString();
            }
        }
    }
}
