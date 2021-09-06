namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameStateViewDictionary : WriteableStateViewDictionary, ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IDictionary<IFrameNodeState, IFrameNodeStateView>, IReadOnlyCollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>, IEqualComparable
    {
        #region IFrameNodeState, IFrameNodeStateView
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this).GetEnumerator(); }

        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return (IFrameNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { List<IFrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { Add(key, value); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { return Remove(key); }
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

            if (!comparer.IsSameType(other, out FrameStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IFrameNodeState Key in Keys)
            {
                IFrameNodeStateView Value = (IFrameNodeStateView)this[Key];

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
