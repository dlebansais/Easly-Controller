namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusStateViewReadOnlyDictionary : FrameStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IDictionary<IFocusNodeState, IFocusNodeStateView>, IReadOnlyCollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusStateViewReadOnlyDictionary(FocusStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IFocusNodeState, IFocusNodeStateView
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Add(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Contains(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.CopyTo(KeyValuePair<IFocusNodeState, IFocusNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Remove(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).GetEnumerator(); }

        IFocusNodeStateView IDictionary<IFocusNodeState, IFocusNodeStateView>.this[IFocusNodeState key] { get { return (IFocusNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFocusNodeState> IDictionary<IFocusNodeState, IFocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusNodeStateView> IDictionary<IFocusNodeState, IFocusNodeStateView>.Values { get { List<IFocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusNodeState, IFocusNodeStateView>.Add(IFocusNodeState key, IFocusNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.Remove(IFocusNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.TryGetValue(IFocusNodeState key, out IFocusNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFocusNodeStateView)Value; return Result; }

        IFocusNodeStateView IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>.this[IFocusNodeState key] { get { return (IFocusNodeStateView)this[key]; } }
        IEnumerable<IFocusNodeState> IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFocusNodeStateView> IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>.Values { get { List<IFocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>.TryGetValue(IFocusNodeState key, out IFocusNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFocusNodeStateView)Value; return Result; }
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
                IFocusNodeStateView Value = (IFocusNodeStateView)this[Key];

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
