namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusNodeStateDictionary : FrameNodeStateDictionary, ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>, IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>, IDictionary<IFocusIndex, IFocusNodeState>, IReadOnlyCollection<KeyValuePair<IFocusIndex, IFocusNodeState>>, IReadOnlyDictionary<IFocusIndex, IFocusNodeState>
    {
        /// <inheritdoc/>
        public FocusNodeStateDictionary() : base() { }
        /// <inheritdoc/>
        public FocusNodeStateDictionary(IDictionary<IFocusIndex, IFocusNodeState> dictionary) : base() { foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FocusNodeStateDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public bool TryGetValue(IFocusIndex key, out IFocusNodeState value) { bool Result = TryGetValue(key, out IFrameNodeState Value); value = (IFocusNodeState)Value; return Result; }

        #region IFocusIndex, IFocusNodeState
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Add(KeyValuePair<IFocusIndex, IFocusNodeState> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Contains(KeyValuePair<IFocusIndex, IFocusNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.CopyTo(KeyValuePair<IFocusIndex, IFocusNodeState>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in this) array[i++] = new KeyValuePair<IFocusIndex, IFocusNodeState>((IFocusIndex)Entry.Key, (IFocusNodeState)Entry.Value); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Remove(KeyValuePair<IFocusIndex, IFocusNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IFocusIndex, IFocusNodeState>((IFocusIndex)iterator.Current.Key, (IFocusNodeState)iterator.Current.Value); } }

        IFocusNodeState IDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return (IFocusNodeState)this[key]; } set { this[key] = value; } }
        ICollection<IFocusIndex> IDictionary<IFocusIndex, IFocusNodeState>.Keys { get { List<IFocusIndex> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusNodeState> IDictionary<IFocusIndex, IFocusNodeState>.Values { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusIndex, IFocusNodeState>.Add(IFocusIndex key, IFocusNodeState value) { Add(key, value); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey(key); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.Remove(IFocusIndex key) { return Remove(key); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value) { bool Result = TryGetValue(key, out IFrameNodeState Value); value = (IFocusNodeState)Value; return Result; }

        IFocusNodeState IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return (IFocusNodeState)this[key]; } }
        IEnumerable<IFocusIndex> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Keys { get { List<IFocusIndex> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusNodeState> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Values { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value) { bool Result = TryGetValue(key, out IFrameNodeState Value); value = (IFocusNodeState)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new FocusNodeStateReadOnlyDictionary(this);
        }
    }
}
