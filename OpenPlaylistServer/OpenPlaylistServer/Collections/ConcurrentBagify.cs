using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace OpenPlaylistServer.Collections {
    public class ConcurrentBagify<t> : ConcurrentBag<t>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Add(t item)
        {
            base.Add(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged == null) return;
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
        }
    }
}
