namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateViewReadOnlyDictionary : ReadOnlyBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IDictionary<IWriteableBlockState, IWriteableBlockStateView>, IReadOnlyCollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>, IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>
    {
        /// <inheritdoc/>
        public WriteableBlockStateViewReadOnlyDictionary(WriteableBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableBlockState, IWriteableBlockStateView
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this).GetEnumerator(); }

        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return (IWriteableBlockStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { List<IWriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue(key, out IReadOnlyBlockStateView Value); value = (IWriteableBlockStateView)Value; return Result; }

        IWriteableBlockStateView IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return (IWriteableBlockStateView)this[key]; } }
        IEnumerable<IWriteableBlockState> IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { List<IWriteableBlockState> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableBlockStateView> IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { List<IWriteableBlockStateView> Result = new(); foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue(key, out IReadOnlyBlockStateView Value); value = (IWriteableBlockStateView)Value; return Result; }
        #endregion
    }
}
