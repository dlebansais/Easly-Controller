#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IWriteableInnerReadOnlyDictionary<TKey> : IReadOnlyInnerReadOnlyDictionary<TKey>, IReadOnlyDictionary<TKey, IWriteableInner>
    {
        new IWriteableInner this[TKey key] { get; }
        new int Count { get; }
        new bool ContainsKey(TKey key);
        new IEnumerator<KeyValuePair<TKey, IWriteableInner>> GetEnumerator();
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class WriteableInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IWriteableInner>, IWriteableInnerReadOnlyDictionary<TKey>
    {
        public WriteableInnerReadOnlyDictionary(IWriteableInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region ReadOnly
        IReadOnlyInner IReadOnlyDictionary<TKey, IReadOnlyInner>.this[TKey key] { get { return this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IReadOnlyInner>.Keys { get { return Keys; } }

        bool IReadOnlyDictionary<TKey, IReadOnlyInner>.TryGetValue(TKey key, out IReadOnlyInner value)
        {
            bool Result = TryGetValue(key, out IWriteableInner Value);
            value = Value;
            return Result;
        }

        IEnumerable<IReadOnlyInner> IReadOnlyDictionary<TKey, IReadOnlyInner>.Values { get { return Values; } }

        IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> IEnumerable<KeyValuePair<TKey, IReadOnlyInner>>.GetEnumerator()
        {
            List<KeyValuePair<TKey, IReadOnlyInner>> NewList = new List<KeyValuePair<TKey, IReadOnlyInner>>();
            foreach (KeyValuePair<TKey, IWriteableInner> Entry in Dictionary)
                NewList.Add(new KeyValuePair<TKey, IReadOnlyInner>(Entry.Key, Entry.Value));

            return NewList.GetEnumerator();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInnerReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IWriteableInner> Entry in this)
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
