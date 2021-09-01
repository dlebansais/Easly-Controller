#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public class WriteableIndexNodeStateDictionary : ReadOnlyIndexNodeStateDictionary, ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IDictionary<IWriteableIndex, IWriteableNodeState>, IReadOnlyCollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>
    {
        #region IWriteableIndex, IWriteableNodeState
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex) { int i = 0; foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in ((ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this)) array[i++] = Entry; }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator() { return new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>(this).GetEnumerator(); }

        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } set { this[key] = value; } }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { Add(key, value); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { return Remove(key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }

        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }
        #endregion
    }
}
