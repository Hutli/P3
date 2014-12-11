using System.Windows;
using System.Windows.Threading;


namespace OpenPlaylistServer {
    public class RootDispatcherFetcher {
        private static Dispatcher _rootDispatcher;

        public static Dispatcher RootDispatcher {
            get
            {
                _rootDispatcher = _rootDispatcher ??
                                  (Application.Current != null
                                      ? Application.Current.Dispatcher
                                      : Dispatcher.CurrentDispatcher);
             return _rootDispatcher;
         }
            // unit tests can get access to this via InternalsVisibleTo
            internal set {
                _rootDispatcher = value;
            }
        }
    }
}
