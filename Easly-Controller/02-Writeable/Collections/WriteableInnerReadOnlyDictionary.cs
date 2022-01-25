namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteableInnerReadOnlyDictionary<TKey> : ReadOnlyInnerReadOnlyDictionary<TKey>, ICollection<KeyValuePair<TKey, IWriteableInner>>, IEnumerable<KeyValuePair<TKey, IWriteableInner>>, IDictionary<TKey, IWriteableInner>, IReadOnlyCollection<KeyValuePair<TKey, IWriteableInner>>, IReadOnlyDictionary<TKey, IWriteableInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableInnerReadOnlyDictionary(WriteableInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out IWriteableInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (IWriteableInner)Value; return Result; }

        #region TKey, IWriteableInner
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Add(KeyValuePair<TKey, IWriteableInner> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this) array[i++] = new KeyValuePair<TKey, IWriteableInner>(Entry.Key, (IWriteableInner)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Remove(KeyValuePair<TKey, IWriteableInner> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IWriteableInner>((TKey)iterator.Current.Key, (IWriteableInner)iterator.Current.Value); } }

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
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableInnerReadOnlyDictionary<TKey> AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IWriteableInner Value = (IWriteableInner)this[Key];

                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
