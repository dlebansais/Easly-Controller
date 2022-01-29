namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameNodeStateDictionary : WriteableNodeStateDictionary, ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>, IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>, IDictionary<IFrameIndex, IFrameNodeState>, IReadOnlyCollection<KeyValuePair<IFrameIndex, IFrameNodeState>>, IReadOnlyDictionary<IFrameIndex, IFrameNodeState>
    {
        /// <inheritdoc/>
        public FrameNodeStateDictionary() : base() { }
        /// <inheritdoc/>
        public FrameNodeStateDictionary(IDictionary<IFrameIndex, IFrameNodeState> dictionary) : base() { foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FrameNodeStateDictionary(int capacity) : base(capacity) { }

        #region IFrameIndex, IFrameNodeState
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Add(KeyValuePair<IFrameIndex, IFrameNodeState> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Contains(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.CopyTo(KeyValuePair<IFrameIndex, IFrameNodeState>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in this) array[i++] = new KeyValuePair<IFrameIndex, IFrameNodeState>((IFrameIndex)Entry.Key, (IFrameNodeState)Entry.Value); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Remove(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IFrameIndex, IFrameNodeState>((IFrameIndex)iterator.Current.Key, (IFrameNodeState)iterator.Current.Value); } }

        IFrameNodeState IDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return (IFrameNodeState)this[key]; } set { this[key] = value; } }
        ICollection<IFrameIndex> IDictionary<IFrameIndex, IFrameNodeState>.Keys { get { List<IFrameIndex> Result = new(); foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in (ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFrameNodeState> IDictionary<IFrameIndex, IFrameNodeState>.Values { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in (ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameIndex, IFrameNodeState>.Add(IFrameIndex key, IFrameNodeState value) { Add(key, value); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey(key); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.Remove(IFrameIndex key) { return Remove(key); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IFrameNodeState)Value; return Result; }

        IFrameNodeState IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return (IFrameNodeState)this[key]; } }
        IEnumerable<IFrameIndex> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Keys { get { List<IFrameIndex> Result = new(); foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in (ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.Values { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in (ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IFrameNodeState)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new FrameNodeStateReadOnlyDictionary(this);
        }
    }
}
