namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusStateViewDictionary : FrameStateViewDictionary, ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IDictionary<IFocusNodeState, IFocusNodeStateView>, IReadOnlyCollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>, IReadOnlyDictionary<IFocusNodeState, IFocusNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public bool TryGetValue(IFocusNodeState key, out IFocusNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFocusNodeStateView)Value; return Result; }

        #region IFocusNodeState, IFocusNodeStateView
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Add(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Contains(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.CopyTo(KeyValuePair<IFocusNodeState, IFocusNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Remove(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).GetEnumerator(); }

        IFocusNodeStateView IDictionary<IFocusNodeState, IFocusNodeStateView>.this[IFocusNodeState key] { get { return (IFocusNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IFocusNodeState> IDictionary<IFocusNodeState, IFocusNodeStateView>.Keys { get { List<IFocusNodeState> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFocusNodeStateView> IDictionary<IFocusNodeState, IFocusNodeStateView>.Values { get { List<IFocusNodeStateView> Result = new(); foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusNodeState, IFocusNodeStateView>.Add(IFocusNodeState key, IFocusNodeStateView value) { Add(key, value); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.Remove(IFocusNodeState key) { return Remove(key); }
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

            if (!comparer.IsSameType(other, out FocusStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IFocusNodeState Key in Keys)
            {
                IFocusNodeStateView Value = (IFocusNodeStateView)this[Key];

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
