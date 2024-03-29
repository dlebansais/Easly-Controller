﻿namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteableNodeStateViewDictionary : ReadOnlyNodeStateViewDictionary, ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IDictionary<IWriteableNodeState, IWriteableNodeStateView>, IReadOnlyCollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableNodeStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public WriteableNodeStateViewDictionary(IDictionary<IWriteableNodeState, IWriteableNodeStateView> dictionary) : base() { foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public WriteableNodeStateViewDictionary(int capacity) : base(capacity) { }

        #region IWriteableNodeState, IWriteableNodeStateView
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this) array[i++] = new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>((IWriteableNodeState)Entry.Key, (IWriteableNodeStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>)this).IsReadOnly; } }
        
        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>((IWriteableNodeState)iterator.Current.Key, (IWriteableNodeStateView)iterator.Current.Value); } }

        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return (IWriteableNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { List<IWriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { Add(key, value); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove(key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IWriteableNodeStateView)Value; return Result; }

        IWriteableNodeStateView IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return (IWriteableNodeStateView)this[key]; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<IWriteableNodeStateView> IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { List<IWriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue(key, out IReadOnlyNodeStateView Value); value = (IWriteableNodeStateView)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateViewReadOnlyDictionary ToReadOnly()
        {
            return new WriteableNodeStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableNodeStateViewDictionary AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableNodeState Key in Keys)
            {
                IWriteableNodeStateView Value = (IWriteableNodeStateView)this[Key];

                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
