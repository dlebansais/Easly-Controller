﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateViewReadOnlyDictionary : ReadOnlyBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IDictionary<IWriteableBlockState, WriteableBlockStateView>, IReadOnlyCollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableBlockStateViewReadOnlyDictionary(WriteableBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(IWriteableBlockState key, out WriteableBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (WriteableBlockStateView)Value; return Result; }

        #region IWriteableBlockState, WriteableBlockStateView
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, WriteableBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this).GetEnumerator(); }

        WriteableBlockStateView IDictionary<IWriteableBlockState, WriteableBlockStateView>.this[IWriteableBlockState key] { get { return (WriteableBlockStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<WriteableBlockStateView> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Values { get { List<WriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableBlockState, WriteableBlockStateView>.Add(IWriteableBlockState key, WriteableBlockStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.Remove(IWriteableBlockState key) { throw new System.InvalidOperationException(); }
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

            if (!comparer.IsSameType(other, out WriteableBlockStateViewReadOnlyDictionary AsBlockStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableBlockState Key in Keys)
            {
                WriteableBlockStateView Value = (WriteableBlockStateView)this[Key];

                if (!comparer.IsTrue(AsBlockStateViewReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}