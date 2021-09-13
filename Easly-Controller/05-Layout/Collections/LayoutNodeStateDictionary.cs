namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutNodeStateDictionary : FocusNodeStateDictionary, ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>, IEnumerable<KeyValuePair<ILayoutIndex, ILayoutNodeState>>, IDictionary<ILayoutIndex, ILayoutNodeState>, IReadOnlyCollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>, IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>
    {
        /// <inheritdoc/>
        public LayoutNodeStateDictionary() : base() { }
        /// <inheritdoc/>
        public LayoutNodeStateDictionary(IDictionary<ILayoutIndex, ILayoutNodeState> dictionary) : base() { foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public LayoutNodeStateDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public bool TryGetValue(ILayoutIndex key, out ILayoutNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = (ILayoutNodeState)Value; return Result; }

        #region ILayoutIndex, ILayoutNodeState
        void ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.Add(KeyValuePair<ILayoutIndex, ILayoutNodeState> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.Contains(KeyValuePair<ILayoutIndex, ILayoutNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.CopyTo(KeyValuePair<ILayoutIndex, ILayoutNodeState>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in this) array[i++] = new KeyValuePair<ILayoutIndex, ILayoutNodeState>((ILayoutIndex)Entry.Key, (ILayoutNodeState)Entry.Value); }
        bool ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.Remove(KeyValuePair<ILayoutIndex, ILayoutNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> IEnumerable<KeyValuePair<ILayoutIndex, ILayoutNodeState>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<ILayoutIndex, ILayoutNodeState>((ILayoutIndex)iterator.Current.Key, (ILayoutNodeState)iterator.Current.Value); } }

        ILayoutNodeState IDictionary<ILayoutIndex, ILayoutNodeState>.this[ILayoutIndex key] { get { return (ILayoutNodeState)this[key]; } set { this[key] = value; } }
        ICollection<ILayoutIndex> IDictionary<ILayoutIndex, ILayoutNodeState>.Keys { get { List<ILayoutIndex> Result = new(); foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in (ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutNodeState> IDictionary<ILayoutIndex, ILayoutNodeState>.Values { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in (ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutIndex, ILayoutNodeState>.Add(ILayoutIndex key, ILayoutNodeState value) { Add(key, value); }
        bool IDictionary<ILayoutIndex, ILayoutNodeState>.ContainsKey(ILayoutIndex key) { return ContainsKey(key); }
        bool IDictionary<ILayoutIndex, ILayoutNodeState>.Remove(ILayoutIndex key) { return Remove(key); }
        bool IDictionary<ILayoutIndex, ILayoutNodeState>.TryGetValue(ILayoutIndex key, out ILayoutNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = (ILayoutNodeState)Value; return Result; }

        ILayoutNodeState IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>.this[ILayoutIndex key] { get { return (ILayoutNodeState)this[key]; } }
        IEnumerable<ILayoutIndex> IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>.Keys { get { List<ILayoutIndex> Result = new(); foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in (ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutNodeState> IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>.Values { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in (ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>.ContainsKey(ILayoutIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<ILayoutIndex, ILayoutNodeState>.TryGetValue(ILayoutIndex key, out ILayoutNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = (ILayoutNodeState)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new LayoutNodeStateReadOnlyDictionary(this);
        }
    }
}
