namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableNodeStateReadOnlyDictionary : ReadOnlyNodeStateReadOnlyDictionary, ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IDictionary<IWriteableIndex, IWriteableNodeState>, IReadOnlyCollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>, IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>
    {
        /// <inheritdoc/>
        public WriteableNodeStateReadOnlyDictionary(WriteableNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableIndex, IWriteableNodeState
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in this) array[i++] = new KeyValuePair<IWriteableIndex, IWriteableNodeState>((IWriteableIndex)Entry.Key, (IWriteableNodeState)Entry.Value); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IWriteableIndex, IWriteableNodeState>((IWriteableIndex)iterator.Current.Key, (IWriteableNodeState)iterator.Current.Value); } }

        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }

        IWriteableNodeState IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return (IWriteableNodeState)this[key]; } }
        IEnumerable<IWriteableIndex> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { List<IWriteableIndex> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IWriteableNodeState)Value; return Result; }
        #endregion
    }
}
