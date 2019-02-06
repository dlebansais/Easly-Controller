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
    public interface IFrameInnerReadOnlyDictionary<TKey> : IWriteableInnerReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IFrameInner>
    {
        new IFrameInner this[TKey key] { get; }
        new int Count { get; }
        new bool ContainsKey(TKey key);
        new IEnumerator<KeyValuePair<TKey, IFrameInner>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class FrameInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameInner>, IFrameInnerReadOnlyDictionary<TKey>
    {
        public FrameInnerReadOnlyDictionary(IFrameInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        IReadOnlyInner IReadOnlyDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<TKey, IReadOnlyInner>.TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }

        IEnumerable<IReadOnlyInner> IReadOnlyDictionary<TKey, IReadOnlyInner>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> IEnumerable<KeyValuePair<TKey, IReadOnlyInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner>>();
            foreach (KeyValuePair<TKey, IFrameInner> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        IWriteableInner IWriteableInnerReadOnlyDictionary<TKey>.this[TKey key] { get { return this[key]; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IWriteableInnerReadOnlyDictionary<TKey>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner>> NewList = new List<KeyValuePair<TKey, IWriteableInner>>();
            foreach (KeyValuePair<TKey, IFrameInner> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }

        IWriteableInner IReadOnlyDictionary<TKey, IWriteableInner>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IWriteableInner>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value)
        {
            bool Result = TryGetValue(key, out IFrameInner Value);
            value = Value;
            return Result;
        }

        IEnumerable<IWriteableInner> IReadOnlyDictionary<TKey, IWriteableInner>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IWriteableInner>> NewList = new List<KeyValuePair<TKey, IWriteableInner>>();
            foreach (KeyValuePair<TKey, IFrameInner> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IWriteableInner>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
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

            if (!comparer.IsSameType(other, out FrameInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameInner> Entry in this)
            {
                if (!comparer.IsTrue(AsInnerReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsInnerReadOnlyDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
