namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutStateViewDictionary : FocusStateViewDictionary, ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>, IEnumerable<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>, IDictionary<ILayoutNodeState, LayoutNodeStateView>, IReadOnlyCollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>, IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>, IEqualComparable
    {
        #region ILayoutNodeState, LayoutNodeStateView
        void ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.Add(KeyValuePair<ILayoutNodeState, LayoutNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.Contains(KeyValuePair<ILayoutNodeState, LayoutNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.CopyTo(KeyValuePair<ILayoutNodeState, LayoutNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.Remove(KeyValuePair<ILayoutNodeState, LayoutNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>> IEnumerable<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>)this).GetEnumerator(); }

        LayoutNodeStateView IDictionary<ILayoutNodeState, LayoutNodeStateView>.this[ILayoutNodeState key] { get { return (LayoutNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<ILayoutNodeState> IDictionary<ILayoutNodeState, LayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, LayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<LayoutNodeStateView> IDictionary<ILayoutNodeState, LayoutNodeStateView>.Values { get { List<LayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, LayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutNodeState, LayoutNodeStateView>.Add(ILayoutNodeState key, LayoutNodeStateView value) { Add(key, value); }
        bool IDictionary<ILayoutNodeState, LayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IDictionary<ILayoutNodeState, LayoutNodeStateView>.Remove(ILayoutNodeState key) { return Remove(key); }
        bool IDictionary<ILayoutNodeState, LayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out LayoutNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (LayoutNodeStateView)Value; return Result; }

        LayoutNodeStateView IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>.this[ILayoutNodeState key] { get { return (LayoutNodeStateView)this[key]; } }
        IEnumerable<ILayoutNodeState> IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, LayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<LayoutNodeStateView> IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>.Values { get { List<LayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, LayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, LayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<ILayoutNodeState, LayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out LayoutNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (LayoutNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (ILayoutNodeState Key in Keys)
            {
                LayoutNodeStateView Value = (LayoutNodeStateView)this[Key];

                if (!comparer.IsTrue(AsStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyStateViewReadOnlyDictionary ToReadOnly()
        {
            return new LayoutStateViewReadOnlyDictionary(this);
        }
    }
}
