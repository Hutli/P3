using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Windows;
using WebAPI;

namespace OpenPlaylistServer.Collections {
    public class ConcurrentDictify<TKey, TVal> : ConcurrentDictionary<TKey, TVal>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(TKey key, TVal value)
        {
            while(!TryAdd(key, value)) { }
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public void Remove(TKey key)
        {
            TVal ignore;
            while(!TryRemove(key, out ignore)) { }
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged == null) return;
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
        }
    }
}
