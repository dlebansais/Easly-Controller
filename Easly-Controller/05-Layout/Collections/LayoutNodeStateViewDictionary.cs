namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutNodeStateViewDictionary : FocusNodeStateViewDictionary, ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IEnumerable<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IDictionary<ILayoutNodeState, ILayoutNodeStateView>, IReadOnlyCollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>, IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutNodeStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public LayoutNodeStateViewDictionary(IDictionary<ILayoutNodeState, ILayoutNodeStateView> dictionary) : base() { foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public LayoutNodeStateViewDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public bool TryGetValue(ILayoutNodeState key, out ILayoutNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = (ILayoutNodeStateView)Value; return Result; }

        #region ILayoutNodeState, ILayoutNodeStateView
        void ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Add(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Contains(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.CopyTo(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.Remove(KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> IEnumerable<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>((ILayoutNodeState)iterator.Current.Key, (ILayoutNodeStateView)iterator.Current.Value); } }

        ILayoutNodeStateView IDictionary<ILayoutNodeState, ILayoutNodeStateView>.this[ILayoutNodeState key] { get { return (ILayoutNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<ILayoutNodeState> IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<ILayoutNodeStateView> IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Values { get { List<ILayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Add(ILayoutNodeState key, ILayoutNodeStateView value) { Add(key, value); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.Remove(ILayoutNodeState key) { return Remove(key); }
        bool IDictionary<ILayoutNodeState, ILayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out ILayoutNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = (ILayoutNodeStateView)Value; return Result; }

        ILayoutNodeStateView IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.this[ILayoutNodeState key] { get { return (ILayoutNodeStateView)this[key]; } }
        IEnumerable<ILayoutNodeState> IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.Keys { get { List<ILayoutNodeState> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<ILayoutNodeStateView> IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.Values { get { List<ILayoutNodeStateView> Result = new(); foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.ContainsKey(ILayoutNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<ILayoutNodeState, ILayoutNodeStateView>.TryGetValue(ILayoutNodeState key, out ILayoutNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = (ILayoutNodeStateView)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateViewReadOnlyDictionary ToReadOnly()
        {
            return new LayoutNodeStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutNodeStateViewDictionary AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (ILayoutNodeState Key in Keys)
            {
                ILayoutNodeStateView Value = (ILayoutNodeStateView)this[Key];

                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
