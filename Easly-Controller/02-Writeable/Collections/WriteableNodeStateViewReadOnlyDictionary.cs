namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteableNodeStateViewReadOnlyDictionary : ReadOnlyNodeStateViewReadOnlyDictionary, ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IDictionary<IWriteableNodeState, IWriteableNodeStateView>, IReadOnlyCollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>, IReadOnlyDictionary<IWriteableNodeState, IWriteableNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteableNodeStateViewReadOnlyDictionary(WriteableNodeStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region IWriteableNodeState, IWriteableNodeStateView
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Clear() { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this) array[i++] = new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>((IWriteableNodeState)Entry.Key, (IWriteableNodeStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { throw new NotSupportedException("Collection is read-only."); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return true; } }

        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>((IWriteableNodeState)iterator.Current.Key, (IWriteableNodeStateView)iterator.Current.Value); } }

        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return (IWriteableNodeStateView)this[key]; } set { throw new NotSupportedException("Collection is read-only."); } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { List<IWriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { throw new NotSupportedException("Collection is read-only."); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { throw new NotSupportedException("Collection is read-only."); }
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableNodeStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableNodeState Key in Keys)
            {
                IWriteableNodeStateView Value = (IWriteableNodeStateView)this[Key];

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
