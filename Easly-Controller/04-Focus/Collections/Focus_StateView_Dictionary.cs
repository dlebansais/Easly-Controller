namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusStateViewDictionary : FrameStateViewDictionary, ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IEnumerable<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IDictionary<IFocusNodeState, FocusNodeStateView>, IReadOnlyCollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>, IReadOnlyDictionary<IFocusNodeState, FocusNodeStateView>, IEqualComparable
    {
        #region IFocusNodeState, FocusNodeStateView
        void ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Add(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Contains(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.CopyTo(KeyValuePair<IFocusNodeState, FocusNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.Remove(KeyValuePair<IFocusNodeState, FocusNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusNodeState, FocusNodeStateView>> IEnumerable<KeyValuePair<IFocusNodeState, FocusNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this).GetEnumerator(); }

        FocusNodeStateView IDictionary<IFocusNodeState, FocusNodeStateView>.this[IFocusNodeState key] { get { return (FocusNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IFocusNodeState> IDictionary<IFocusNodeState, FocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FocusNodeStateView> IDictionary<IFocusNodeState, FocusNodeStateView>.Values { get { List<FocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, FocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, FocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusNodeState, FocusNodeStateView>.Add(IFocusNodeState key, FocusNodeStateView value) { Add(key, value); }
        bool IDictionary<IFocusNodeState, FocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFocusNodeState, FocusNodeStateView>.Remove(IFocusNodeState key) { return Remove(key); }
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

            if (!comparer.IsSameType(other, out FocusStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IFocusNodeState Key in Keys)
            {
                FocusNodeStateView Value = (FocusNodeStateView)this[Key];

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
            return new FocusStateViewReadOnlyDictionary(this);
        }
    }
}
