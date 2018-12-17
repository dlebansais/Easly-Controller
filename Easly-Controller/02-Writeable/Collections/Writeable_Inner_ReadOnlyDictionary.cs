using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IWriteableInnerReadOnlyDictionary<TKey> : IReadOnlyInnerReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>
    {
        new IWriteableInner<IWriteableBrowsingChildIndex> this[TKey key] { get; }
        new IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class WriteableInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>, IWriteableInnerReadOnlyDictionary<TKey>
    {
        public WriteableInnerReadOnlyDictionary(IWriteableInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        public new IReadOnlyInner<IReadOnlyBrowsingChildIndex> this[TKey key] { get { return base[key]; } }
        public new IEnumerable<TKey> Keys { get { return base.Keys; } }
        public new IEnumerable<IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Values { get { return base.Values; } }
        public new IEnumerator<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>();
            foreach (KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out IReadOnlyInner<IReadOnlyBrowsingChildIndex> value) { bool Result = TryGetValue(key, out IWriteableInner<IWriteableBrowsingChildIndex> Value); value = Value; return Result; }
    }
}
