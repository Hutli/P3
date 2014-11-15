using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace OpenPlaylistServer
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return ObjectFactory.Container;
        }
    }
}
