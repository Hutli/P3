using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.TinyIoc;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return ObjectFactory.Container;
        }

        public static void Initialise()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IMainWindow>().Use<MainWindow>();
                x.For<IMainWindowViewModel>().Use<MainWindowViewModel>();

                x.ForSingletonOf<IPlaylistService>().Use<PlaylistService>();
                x.ForSingletonOf<IVoteService>().Use<VoteService>();
                x.ForSingletonOf<IUserService>().Use<UserService>();
                x.ForSingletonOf<IPlaybackService>().Use<PlaybackService>();
            });
        }
    }
}
