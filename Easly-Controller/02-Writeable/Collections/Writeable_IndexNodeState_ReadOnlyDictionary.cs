namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableIndexNodeStateReadOnlyDictionary : ReadOnlyIndexNodeStateReadOnlyDictionary, ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IDictionary<IWriteableIndex, IWriteableNodeState>, IReadOnlyCollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>
    {
        /// <inheritdoc/>
        public WriteableIndexNodeStateReadOnlyDictionary(WriteableIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableIndex, IWriteableNodeState
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).GetEnumerator(); }

        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }

        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }
        #endregion
    }
}
