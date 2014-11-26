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
        void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }
    }
}
