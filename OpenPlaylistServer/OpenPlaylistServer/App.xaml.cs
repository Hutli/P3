using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nancy.TinyIoc;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Bootstrapper.Initialise();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            IContainer container = ObjectFactory.Container;
            
            MainWindow = (Window)container.GetInstance<IMainWindow>();
            MainWindow.ShowDialog();
        }
    }


}
