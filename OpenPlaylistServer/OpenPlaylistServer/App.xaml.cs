using System;
using System.Threading;
using System.Windows;
using OpenPlaylistServer.Services.Implementation;
using OpenPlaylistServer.Services.Interfaces;
using StructureMap;

namespace OpenPlaylistServer {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        protected override void OnStartup(StartupEventArgs e) {
            var container = ObjectFactory.Container;

            MainWindow = (Window)container.GetInstance<IMainWindow>();
            MainWindow.ShowDialog();
        }
    }

    public static class ObjectFactory {
        public static IContainer Container {
            get {return ContainerBuilder.Value;}
        }

        private static Container DefaultContainer() {
            return new Container(x => {
                x.For<IMainWindow>().Use<MainWindow>();
                x.For<IMainWindowViewModel>().Use<MainWindowViewModel>();

                x.ForSingletonOf<IPlaylistService>().Use<PlaylistService>();
                x.ForSingletonOf<IVoteService>().Use<VoteService>();
                x.ForSingletonOf<IUserService>().Use<UserService>();
                x.ForSingletonOf<IPlaybackService>().Use<PlaybackService>();
                x.For<ISearchService>().Use<SearchService>();
                x.ForSingletonOf<IRestrictionService>().Use<RestrictionService>();
                x.ForSingletonOf<IHistoryService>().Use<HistoryService>();
            });
        }

        private static readonly Lazy<Container> ContainerBuilder = new Lazy<Container>(DefaultContainer,
                                                                                       LazyThreadSafetyMode
                                                                                           .ExecutionAndPublication);
    }
}