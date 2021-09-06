namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutStateViewReadOnlyDictionary : FocusStateViewReadOnlyDictionary, ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IEnumerable<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IDictionary<ILayoutNodeState, ILayoutNodeStateView>, IReadOnlyCollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutStateViewReadOnlyDictionary(LayoutStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region ILayoutNodeState, ILayoutNodeStateView
        void ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Add(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Contains(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.CopyTo(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Remove(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> IEnumerable<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this).GetEnumerator(); }

        ILayoutNodeStateView IDictionary<ILayoutNodeState, ILayoutNodeStateView>.this[ILayoutNodeState key] { get { return (ILayoutNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<ILayoutNodeState> IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutNodeStateView> IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Values { get { List<ILayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Add(ILayoutNodeState key, ILayoutNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Remove(ILayoutNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out ILayoutNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (ILayoutNodeStateView)Value; return Result; }

        ILayoutNodeStateView IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.this[ILayoutNodeState key] { get { return (ILayoutNodeStateView)this[key]; } }
        IEnumerable<ILayoutNodeState> IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutNodeStateView> IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.Values { get { List<ILayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out ILayoutNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (ILayoutNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutStateViewReadOnlyDictionary AsStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (ILayoutNodeState Key in Keys)
            {
                ILayoutNodeStateView Value = (ILayoutNodeStateView)this[Key];

                if (!comparer.IsTrue(AsStateViewReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsStateViewReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
