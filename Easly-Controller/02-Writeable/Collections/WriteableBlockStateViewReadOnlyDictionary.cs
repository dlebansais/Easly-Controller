namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteableBlockStateViewReadOnlyDictionary : ReadOnlyBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IDictionary<IWriteableBlockState, WriteableBlockStateView>, IReadOnlyCollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>, IReadOnlyDictionary<IWriteableBlockState, WriteableBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableBlockStateViewReadOnlyDictionary(WriteableBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableBlockState, WriteableBlockStateView
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, WriteableBlockStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this) array[i++] = new KeyValuePair<IWriteableBlockState, WriteableBlockStateView>((IWriteableBlockState)Entry.Key, (WriteableBlockStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, WriteableBlockStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IWriteableBlockState, WriteableBlockStateView>((IWriteableBlockState)iterator.Current.Key, (WriteableBlockStateView)iterator.Current.Value); } }

        WriteableBlockStateView IDictionary<IWriteableBlockState, WriteableBlockStateView>.this[IWriteableBlockState key] { get { return (WriteableBlockStateView)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<WriteableBlockStateView> IDictionary<IWriteableBlockState, WriteableBlockStateView>.Values { get { List<WriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableBlockState, WriteableBlockStateView>.Add(IWriteableBlockState key, WriteableBlockStateView value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableBlockState, WriteableBlockStateView>.Remove(IWriteableBlockState key) { throw new NotSupportedException("Collection is read-only."); }
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableBlockStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableBlockState Key in Keys)
            {
                WriteableBlockStateView Value = (WriteableBlockStateView)this[Key];

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
