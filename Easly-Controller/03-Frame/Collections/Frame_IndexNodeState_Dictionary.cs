namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameIndexNodeStateDictionary : WriteableIndexNodeStateDictionary, ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>, IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>, IDictionary<IFrameIndex, IFrameNodeState>, IReadOnlyCollection<KeyValuePair<IFrameIndex, IFrameNodeState>>, IReadOnlyDictionary<IFrameIndex, IFrameNodeState>
    {
        #region IFrameIndex, IFrameNodeState
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Add(KeyValuePair<IFrameIndex, IFrameNodeState> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Contains(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.CopyTo(KeyValuePair<IFrameIndex, IFrameNodeState>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Remove(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).GetEnumerator(); }

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
        public override ReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new FrameIndexNodeStateReadOnlyDictionary(this);
        }
    }
}
