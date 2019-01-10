using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IFocusInnerReadOnlyDictionary<TKey> : IFrameInnerReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IFocusInner<IFocusBrowsingChildIndex>>
    {
        new int Count { get; }
        new IFocusInner<IFocusBrowsingChildIndex> this[TKey key] { get; }
        new IEnumerator<KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>>> GetEnumerator();
        new bool ContainsKey(TKey key);
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class FocusInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFocusInner<IFocusBrowsingChildIndex>>, IFocusInnerReadOnlyDictionary<TKey>
    {
        public FocusInnerReadOnlyDictionary(IFocusInnerDictionary<TKey> dictionary)
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
            foreach (KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out IReadOnlyInner<IReadOnlyBrowsingChildIndex> value) { bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value); value = Value; return Result; }
        #endregion

        #region Writeable
        IWriteableInner<IWriteableBrowsingChildIndex> IWriteableInnerReadOnlyDictionary<TKey>.this[TKey key] { get { return base[key]; } }
        IWriteableInner<IWriteableBrowsingChildIndex> IReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.this[TKey key] { get { return base[key]; } }
        IEnumerable<IWriteableInner<IWriteableBrowsingChildIndex>> IReadOnlyDictionary<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>.Values { get { return base.Values; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> IWriteableInnerReadOnlyDictionary<TKey>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>();
            foreach (KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>> IEnumerable<KeyValuePair<TKey, IWriteableInner<IWriteableBrowsingChildIndex>>>.GetEnumerator() { return ((IWriteableInnerReadOnlyDictionary<TKey>)this).GetEnumerator(); }

        public bool TryGetValue(TKey key, out IWriteableInner<IWriteableBrowsingChildIndex> value) { bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value); value = Value; return Result; }
        #endregion

        #region Frame
        IFrameInner<IFrameBrowsingChildIndex> IFrameInnerReadOnlyDictionary<TKey>.this[TKey key] { get { return base[key]; } }
        IFrameInner<IFrameBrowsingChildIndex> IReadOnlyDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.this[TKey key] { get { return base[key]; } }
        IEnumerable<IFrameInner<IFrameBrowsingChildIndex>> IReadOnlyDictionary<TKey, IFrameInner<IFrameBrowsingChildIndex>>.Values { get { return base.Values; } }

        IEnumerator<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> IFrameInnerReadOnlyDictionary<TKey>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> NewList = new List<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>>();
            foreach (KeyValuePair<TKey, IFocusInner<IFocusBrowsingChildIndex>> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        IEnumerator<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>> IEnumerable<KeyValuePair<TKey, IFrameInner<IFrameBrowsingChildIndex>>>.GetEnumerator() { return ((IFrameInnerReadOnlyDictionary<TKey>)this).GetEnumerator(); }

        public bool TryGetValue(TKey key, out IFrameInner<IFrameBrowsingChildIndex> value) { bool Result = TryGetValue(key, out IFocusInner<IFocusBrowsingChildIndex> Value); value = Value; return Result; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusInnerReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return false;

            if (Count != AsInnerReadOnlyDictionary.Count)
                return false;

            foreach (KeyValuePair<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in this)
            {
                if (!AsInnerReadOnlyDictionary.ContainsKey(Entry.Key))
                    return false;

                if (!comparer.VerifyEqual(Entry.Value, AsInnerReadOnlyDictionary[Entry.Key]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
