using System.Windows;
using System.Windows.Threading;

namespace OpenPlaylistServer {
    public static class RootDispatcherFetcher {
        private static Dispatcher _rootDispatcher;

        public static Dispatcher RootDispatcher {
            get {
                return _rootDispatcher
                       ?? (Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher);
            }
            // unit tests can get access to this via InternalsVisibleTo
            internal set {_rootDispatcher = value;}
        }
    }
}