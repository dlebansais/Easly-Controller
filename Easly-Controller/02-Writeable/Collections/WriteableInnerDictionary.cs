﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteableInnerDictionary<TKey> : ReadOnlyInnerDictionary<TKey>, ICollection<KeyValuePair<TKey, IWriteableInner>>, IEnumerable<KeyValuePair<TKey, IWriteableInner>>, IDictionary<TKey, IWriteableInner>, IReadOnlyCollection<KeyValuePair<TKey, IWriteableInner>>, IReadOnlyDictionary<TKey, IWriteableInner>, IEqualComparable
        where TKey : notnull
    {
        /// <inheritdoc/>
        public WriteableInnerDictionary() : base() { }
        /// <inheritdoc/>
        public WriteableInnerDictionary(IDictionary<TKey, IWriteableInner> dictionary) : base() { foreach (KeyValuePair<TKey, IWriteableInner> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public WriteableInnerDictionary(int capacity) : base(capacity) { }

        #region TKey, IWriteableInner
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.Add(KeyValuePair<TKey, IWriteableInner> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Contains(KeyValuePair<TKey, IWriteableInner> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<TKey, IWriteableInner>>.CopyTo(KeyValuePair<TKey, IWriteableInner>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this) array[i++] = new KeyValuePair<TKey, IWriteableInner>(Entry.Key, (IWriteableInner)Entry.Value); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.Remove(KeyValuePair<TKey, IWriteableInner> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<TKey, IWriteableInner>>.IsReadOnly { get { return ((ICollection<KeyValuePair<TKey, IReadOnlyInner>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<TKey, IWriteableInner>> IEnumerable<KeyValuePair<TKey, IWriteableInner>>.GetEnumerator() { IEnumerator<KeyValuePair<TKey, IReadOnlyInner>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<TKey, IWriteableInner>((TKey)iterator.Current.Key, (IWriteableInner)iterator.Current.Value); } }

        IWriteableInner IDictionary<TKey, IWriteableInner>.this[TKey key] { get { return (IWriteableInner)this[key]; } set { this[key] = value; } }
        ICollection<TKey> IDictionary<TKey, IWriteableInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableInner> IDictionary<TKey, IWriteableInner>.Values { get { List<IWriteableInner> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<TKey, IWriteableInner>.Add(TKey key, IWriteableInner value) { Add(key, value); }
        bool IDictionary<TKey, IWriteableInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IDictionary<TKey, IWriteableInner>.Remove(TKey key) { return Remove(key); }
        bool IDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (IWriteableInner)Value; return Result; }

        IWriteableInner IReadOnlyDictionary<TKey, IWriteableInner>.this[TKey key] { get { return (IWriteableInner)this[key]; } }
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IWriteableInner>.Keys { get { List<TKey> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableInner> IReadOnlyDictionary<TKey, IWriteableInner>.Values { get { List<IWriteableInner> Result = new(); foreach (KeyValuePair<TKey, IWriteableInner> Entry in (ICollection<KeyValuePair<TKey, IWriteableInner>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<TKey, IWriteableInner>.ContainsKey(TKey key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<TKey, IWriteableInner>.TryGetValue(TKey key, out IWriteableInner value) { bool Result = TryGetValue(key, out IReadOnlyInner Value); value = (IWriteableInner)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new WriteableInnerReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableInnerDictionary<TKey> AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (TKey Key in Keys)
            {
                IWriteableInner Value = (IWriteableInner)this[Key];

                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
