namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameStateViewReadOnlyDictionary : WriteableStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IEnumerable<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IDictionary<IFrameNodeState, FrameNodeStateView>, IReadOnlyCollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameStateViewReadOnlyDictionary(FrameStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IFrameNodeState, FrameNodeStateView
        void ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, FrameNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameNodeState, FrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this).GetEnumerator(); }

        FrameNodeStateView IDictionary<IFrameNodeState, FrameNodeStateView>.this[IFrameNodeState key] { get { return (FrameNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, FrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FrameNodeStateView> IDictionary<IFrameNodeState, FrameNodeStateView>.Values { get { List<FrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameNodeState, FrameNodeStateView>.Add(IFrameNodeState key, FrameNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, FrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFrameNodeState, FrameNodeStateView>.Remove(IFrameNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, FrameNodeStateView>.TryGetValue(IFrameNodeState key, out FrameNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (FrameNodeStateView)Value; return Result; }

        FrameNodeStateView IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>.this[IFrameNodeState key] { get { return (FrameNodeStateView)this[key]; } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<FrameNodeStateView> IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>.Values { get { List<FrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>.TryGetValue(IFrameNodeState key, out FrameNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (FrameNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameStateViewReadOnlyDictionary AsStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IFrameNodeState Key in Keys)
            {
                FrameNodeStateView Value = (FrameNodeStateView)this[Key];

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
