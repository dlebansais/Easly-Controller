﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableStateViewReadOnlyDictionary : ReadOnlyStateViewReadOnlyDictionary, ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IDictionary<IWriteableNodeState, IWriteableNodeStateView>, IReadOnlyCollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableStateViewReadOnlyDictionary(WriteableStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableNodeState, IWriteableNodeStateView
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this).GetEnumerator(); }

        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return (IWriteableNodeStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { List<IWriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IWriteableNodeStateView)Value; return Result; }

        IWriteableNodeStateView IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return (IWriteableNodeStateView)this[key]; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableNodeStateView> IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { List<IWriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IWriteableNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableStateViewReadOnlyDictionary AsStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableNodeState Key in Keys)
            {
                IWriteableNodeStateView Value = (IWriteableNodeStateView)this[Key];

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
