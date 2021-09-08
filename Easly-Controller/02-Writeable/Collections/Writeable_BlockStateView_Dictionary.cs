﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateViewDictionary : ReadOnlyBlockStateViewDictionary, ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IDictionary<IWriteableBlockState, WriteableBlockStateView>, IReadOnlyCollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public bool TryGetValue(IWriteableBlockState key, out WriteableBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (WriteableBlockStateView)Value; return Result; }

        #region IWriteableBlockState, WriteableBlockStateView
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, WriteableBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this).GetEnumerator(); }

        WriteableBlockStateView IDictionary<IWriteableBlockState, WriteableBlockStateView>.this[IWriteableBlockState key] { get { return (WriteableBlockStateView)this[key]; } set { this[key] = value; } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<WriteableBlockStateView> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Values { get { List<WriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableBlockState, WriteableBlockStateView>.Add(IWriteableBlockState key, WriteableBlockStateView value) { Add(key, value); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove(key); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out WriteableBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (WriteableBlockStateView)Value; return Result; }

        WriteableBlockStateView IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>.this[IWriteableBlockState key] { get { return (WriteableBlockStateView)this[key]; } }
        IEnumerable<IWriteableBlockState> IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<WriteableBlockStateView> IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>.Values { get { List<WriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out WriteableBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (WriteableBlockStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableBlockState Key in Keys)
            {
                WriteableBlockStateView Value = (WriteableBlockStateView)this[Key];

                if (!comparer.IsTrue(AsBlockStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new WriteableBlockStateViewReadOnlyDictionary(this);
        }
    }
}
