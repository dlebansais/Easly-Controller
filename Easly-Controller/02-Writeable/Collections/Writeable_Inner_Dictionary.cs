using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    public interface IWriteableInnerDictionary<TKey> : IReadOnlyInnerDictionary<TKey>, IDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    public class WriteableInnerDictionary<TKey> : Dictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>, IWriteableInnerDictionary<TKey>
    {
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.this[TKey key] { get { return this[key]; } set { this[key] = (IWriteableInner<IWriteableBrowsingChildIndex>)value; } }
        ICollection<TKey> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Keys { get { return Keys; } }
        ICollection<IReadOnlyInner<IReadOnlyBrowsingChildIndex>> IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Values { get { return new List<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Values); } }
        public void Add(TKey key, IReadOnlyInner<IReadOnlyBrowsingChildIndex> value) { base.Add(key, (IWriteableInner<IWriteableBrowsingChildIndex>)value); }

        public new IEnumerator<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>();
            IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> Enumerator = base.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out IReadOnlyInner<IReadOnlyBrowsingChildIndex> value) { bool Result = TryGetValue(key, out IWriteableInner<IWriteableBrowsingChildIndex> Value); value = Value; return Result; }
        public void Add(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { base.Add(item.Key, (IWriteableInner<IWriteableBrowsingChildIndex>)item.Value); }
        public bool Contains(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IWriteableInner>>)this).IsReadOnly; } }
    }
}
