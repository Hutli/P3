using OpenPlaylistServer.Services.Implementation;
using OpenPlaylistServer.Services.Interfaces;
using StructureMap;
using System.Windows;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IMainWindow>().Use<MainWindow>();
                x.For<IMainWindowViewModel>().Use<MainWindowViewModel>();

                x.ForSingletonOf<IPlaylistService>().Use<PlaylistService>();
                x.ForSingletonOf<IVoteService>().Use<VoteService>();
                x.ForSingletonOf<IUserService>().Use<UserService>();
                x.ForSingletonOf<IPlaybackService>().Use<PlaybackService>();
                x.For<ISearchService>().Use<SearchService>();
                x.For<IRestrictionService>().Use<RestrictionService>();
                x.For<IHistoryService>().Use<HistoryService>();
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            IContainer container = ObjectFactory.Container;
            
            MainWindow = (Window)container.GetInstance<IMainWindow>();
            MainWindow.ShowDialog();
        }
    }


}
