namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusStateViewReadOnlyDictionary : FrameStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IEnumerable<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IDictionary<IFocusNodeState, FocusNodeStateView>, IReadOnlyCollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusStateViewReadOnlyDictionary(FocusStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IFocusNodeState, FocusNodeStateView
        void ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Add(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Contains(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.CopyTo(KeyValuePair<IFocusNodeState, FocusNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Remove(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusNodeState, FocusNodeStateView>> IEnumerable<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this).GetEnumerator(); }

        FocusNodeStateView IDictionary<IFocusNodeState, FocusNodeStateView>.this[IFocusNodeState key] { get { return (FocusNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFocusNodeState> IDictionary<IFocusNodeState, FocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FocusNodeStateView> IDictionary<IFocusNodeState, FocusNodeStateView>.Values { get { List<FocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusNodeState, FocusNodeStateView>.Add(IFocusNodeState key, FocusNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusNodeState, FocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFocusNodeState, FocusNodeStateView>.Remove(IFocusNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusNodeState, FocusNodeStateView>.TryGetValue(IFocusNodeState key, out FocusNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (FocusNodeStateView)Value; return Result; }

        FocusNodeStateView IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>.this[IFocusNodeState key] { get { return (FocusNodeStateView)this[key]; } }
        IEnumerable<IFocusNodeState> IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<FocusNodeStateView> IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>.Values { get { List<FocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>.TryGetValue(IFocusNodeState key, out FocusNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (FocusNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusStateViewReadOnlyDictionary AsStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IFocusNodeState Key in Keys)
            {
                FocusNodeStateView Value = (FocusNodeStateView)this[Key];

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
