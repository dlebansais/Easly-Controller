namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusIndexNodeStateReadOnlyDictionary : FrameIndexNodeStateReadOnlyDictionary, ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>, IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>, IDictionary<IFocusIndex, IFocusNodeState>, IReadOnlyCollection<KeyValuePair<IFocusIndex, IFocusNodeState>>, IReadOnlyDictionary<IFocusIndex, IFocusNodeState>
    {
        /// <inheritdoc/>
        public FocusIndexNodeStateReadOnlyDictionary(FocusIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IFocusIndex, IFocusNodeState
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Add(KeyValuePair<IFocusIndex, IFocusNodeState> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Contains(KeyValuePair<IFocusIndex, IFocusNodeState> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.CopyTo(KeyValuePair<IFocusIndex, IFocusNodeState>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Remove(KeyValuePair<IFocusIndex, IFocusNodeState> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).GetEnumerator(); }

        IFocusNodeState IDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return (IFocusNodeState)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFocusIndex> IDictionary<IFocusIndex, IFocusNodeState>.Keys { get { List<IFocusIndex> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusNodeState> IDictionary<IFocusIndex, IFocusNodeState>.Values { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusIndex, IFocusNodeState>.Add(IFocusIndex key, IFocusNodeState value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey(key); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.Remove(IFocusIndex key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IFocusNodeState)Value; return Result; }

        IFocusNodeState IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return (IFocusNodeState)this[key]; } }
        IEnumerable<IFocusIndex> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Keys { get { List<IFocusIndex> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusNodeState> IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.Values { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value) { bool Result = TryGetValue(key, out IReadOnlyNodeState Value); value = (IFocusNodeState)Value; return Result; }
        #endregion
    }
}
