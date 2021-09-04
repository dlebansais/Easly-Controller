namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameStateViewDictionary : WriteableStateViewDictionary, ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IEnumerable<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IDictionary<IFrameNodeState, FrameNodeStateView>, IReadOnlyCollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>, IReadOnlyDictionary<IFrameNodeState, FrameNodeStateView>, IEqualComparable
    {
        #region IFrameNodeState, FrameNodeStateView
        void ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, FrameNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, FrameNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameNodeState, FrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, FrameNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this).GetEnumerator(); }

        FrameNodeStateView IDictionary<IFrameNodeState, FrameNodeStateView>.this[IFrameNodeState key] { get { return (FrameNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, FrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FrameNodeStateView> IDictionary<IFrameNodeState, FrameNodeStateView>.Values { get { List<FrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, FrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, FrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameNodeState, FrameNodeStateView>.Add(IFrameNodeState key, FrameNodeStateView value) { Add(key, value); }
        bool IDictionary<IFrameNodeState, FrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFrameNodeState, FrameNodeStateView>.Remove(IFrameNodeState key) { return Remove(key); }
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

            if (!comparer.IsSameType(other, out FrameStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IFrameNodeState Key in Keys)
            {
                FrameNodeStateView Value = (FrameNodeStateView)this[Key];

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
            return new FrameStateViewReadOnlyDictionary(this);
        }
    }
}
