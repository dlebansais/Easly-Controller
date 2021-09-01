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
    public class WriteableInnerReadOnlyDictionary<TKey> : ReadOnlyInnerReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, IWriteableInner>>, IEnumerable<KeyValuePair<TKey, IWriteableInner>>, IDictionary<TKey, IWriteableInner>, IReadOnlyCollection<KeyValuePair<TKey, IWriteableInner>>, IReadOnlyDictionary<TKey, IWriteableInner>, IEqualComparable
    {
        public WriteableInnerReadOnlyDictionary(WriteableInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region TKey, IWriteableInner
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Add(KeyValuePair<TKey, IWriteableInner> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex) { int i = 0; foreach (KeyValuePair<TKey, IWriteableInner> Entry in ((ICollection<KeyValuePair<TKey, IWriteableInner>>)this)) array[i++] = Entry; }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Remove(KeyValuePair<TKey, IWriteableInner> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator() { return new List<KeyValuePair<TKey, IWriteableInner>>(this).GetEnumerator(); }

        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return (IWriteableInner)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { List<IWriteableInner> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IWriteableInner>.Add(TKey key, IWriteableInner value) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IWriteableInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IWriteableInner>.Remove(TKey key) { throw new System.InvalidOperationException(); }
        bool IDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (IWriteableInner)Value; return Result; }

        IWriteableInner IReadOnlyDictionary<TKey, IWriteableInner>.this[TKey key] { get { return (IWriteableInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IWriteableInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableInner> IReadOnlyDictionary<TKey, IWriteableInner>.Values { get { List<IWriteableInner> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IWriteableInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (IWriteableInner)Value; return Result; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInnerReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IWriteableInner Value = (IWriteableInner)this[Key];

                if (!comparer.IsTrue(AsInnerReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsInnerReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
