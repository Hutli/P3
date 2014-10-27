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

            //var container = new TinyIoCContainer();
            //container.AutoRegister();
            //container.Register<IMainWindow>().UsingConstructor<MainWindow>( () => { return new MainWindow(IVoteService });
            //container.Register<IMainWindow, MainWindow>();
            //var test = container.Resolve<MainWindow>();
            //container.Resolve<IMainWindow>();

            //MainWindow = (Window)container.Resolve<MainWindow>();

            Bootstrapper.Initialise();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            IContainer container = ObjectFactory.Container;
            
            MainWindow = (Window)container.GetInstance<IMainWindow>();
            MainWindow.ShowDialog();
            //MainWindow = (Window) container.Resolve<IMainWindow>();
        }
    }


}
