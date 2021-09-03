namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateViewDictionary : ReadOnlyBlockStateViewDictionary, ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IDictionary<IWriteableBlockState, IWriteableBlockStateView>, IReadOnlyCollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>, IEqualComparable
    {
        #region IWriteableBlockState, IWriteableBlockStateView
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this).GetEnumerator(); }

        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return (IWriteableBlockStateView)this[key]; } set { this[key] = value; } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { List<IWriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { Add(key, value); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove(key); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue(key, out IReadOnlyBlockStateView Value); value = (IWriteableBlockStateView)Value; return Result; }

        IWriteableBlockStateView IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return (IWriteableBlockStateView)this[key]; } }
        IEnumerable<IWriteableBlockState> IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableBlockStateView> IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { List<IWriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue(key, out IReadOnlyBlockStateView Value); value = (IWriteableBlockStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableBlockState Key in Keys)
            {
                IWriteableBlockStateView Value = (IWriteableBlockStateView)this[Key];

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
