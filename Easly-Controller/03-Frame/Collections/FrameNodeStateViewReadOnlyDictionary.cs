﻿namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameNodeStateViewReadOnlyDictionary : WriteableNodeStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IDictionary<IFrameNodeState, IFrameNodeStateView>, IReadOnlyCollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>, IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameNodeStateViewReadOnlyDictionary(FrameNodeStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IWriteableNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }

        #region IFrameNodeState, IFrameNodeStateView
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this) array[i++] = new KeyValuePair<IFrameNodeState, IFrameNodeStateView>((IFrameNodeState)Entry.Key, (IFrameNodeStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IFrameNodeState, IFrameNodeStateView>((IFrameNodeState)iterator.Current.Key, (IFrameNodeStateView)iterator.Current.Value); } }

        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return (IFrameNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { List<IFrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IWriteableNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }

        IFrameNodeStateView IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return (IFrameNodeStateView)this[key]; } }
        IEnumerable<IFrameNodeState> IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { List<IFrameNodeState> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IFrameNodeStateView> IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { List<IFrameNodeStateView> Result = new(); foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in (ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IWriteableNodeStateView Value); value = (IFrameNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameNodeStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IFrameNodeState Key in Keys)
            {
                IFrameNodeStateView Value = (IFrameNodeStateView)this[Key];

                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}