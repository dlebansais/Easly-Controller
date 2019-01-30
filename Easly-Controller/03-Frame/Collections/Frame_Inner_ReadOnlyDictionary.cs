#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameInnerReadOnlyDictionary<TKey> : IWriteableInnerReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>
    {
        new int Count { get; }
        new IFrameInner<IFrameBrowsingChildIndex> this[TKey key] { get; }
        new IEnumerator<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> GetEnumerator();
        new bool ContainsKey(TKey key);
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>, IFrameInnerReadOnlyDictionary<TKey>
    {
        public FrameInnerReadOnlyDictionary(IFrameInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        public new IReadOnlyInner<IReadOnlyBrowsingChildIndex> this[TKey key] { get { return base[key]; } }
        public new IEnumerable<TKey> Keys { get { return base.Keys; } }
        public new IEnumerable<IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Values { get { return base.Values; } }

        public new IEnumerator<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>>();
            foreach (KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out IReadOnlyInner<IReadOnlyBrowsingChildIndex> value)
        {
            bool Result = TryGetValue(key, out IFrameInner<IFrameBrowsingChildIndex> Value);
            value = Value;
            return Result;
        }
        #endregion

        #region Writeable
        IWriteableInner<IWriteableBrowsingChildIndex> IWriteableInnerReadOnlyDictionary<TKey>.this[TKey key] { get { return base[key]; } }
        IWriteableInner<IWriteableBrowsingChildIndex> IReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.this[TKey key] { get { return base[key]; } }
        IEnumerable<IWriteableInner<IWriteableBrowsingChildIndex>> IReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.Values { get { return base.Values; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> IWriteableInnerReadOnlyDictionary<TKey>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>();
            foreach (KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> IEnumerable<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>.GetEnumerator() { return ((IWriteableInnerReadOnlyDictionary<TKey>)this).GetEnumerator(); }

        public bool TryGetValue(TKey key, out IWriteableInner<IWriteableBrowsingChildIndex> value)
        {
            bool Result = TryGetValue(key, out IFrameInner<IFrameBrowsingChildIndex> Value);
            value = Value;
            return Result;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameInnerReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (Count != AsInnerReadOnlyDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in this)
            {
                if (!AsInnerReadOnlyDictionary.ContainsKey(Entry.Key))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsInnerReadOnlyDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
