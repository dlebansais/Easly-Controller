namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameStateViewReadOnlyDictionary : WriteableStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IDictionary<IFrameNodeState, IFrameNodeStateView>, IReadOnlyCollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameStateViewReadOnlyDictionary(FrameStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }

        #region IFrameNodeState, IFrameNodeStateView
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this).GetEnumerator(); }

        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return (IFrameNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { List<IFrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }

        IFrameNodeStateView IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return (IFrameNodeStateView)this[key]; } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFrameNodeStateView> IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { List<IFrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }
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
                IFrameNodeStateView Value = (IFrameNodeStateView)this[Key];

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
