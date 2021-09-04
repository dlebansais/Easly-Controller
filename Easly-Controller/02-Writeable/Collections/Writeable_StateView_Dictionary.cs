namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableStateViewDictionary : ReadOnlyStateViewDictionary, ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>, IEnumerable<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>, IDictionary<IWriteableNodeState, WriteableNodeStateView>, IReadOnlyCollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>, IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>, IEqualComparable
    {
        #region IWriteableNodeState, WriteableNodeStateView
        void ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, WriteableNodeStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, WriteableNodeStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, WriteableNodeStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, WriteableNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>)this).GetEnumerator(); }

        WriteableNodeStateView IDictionary<IWriteableNodeState, WriteableNodeStateView>.this[IWriteableNodeState key] { get { return (WriteableNodeStateView)this[key]; } set { this[key] = value; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, WriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, WriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<WriteableNodeStateView> IDictionary<IWriteableNodeState, WriteableNodeStateView>.Values { get { List<WriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, WriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IWriteableNodeState, WriteableNodeStateView>.Add(IWriteableNodeState key, WriteableNodeStateView value) { Add(key, value); }
        bool IDictionary<IWriteableNodeState, WriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IDictionary<IWriteableNodeState, WriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove(key); }
        bool IDictionary<IWriteableNodeState, WriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out WriteableNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (WriteableNodeStateView)Value; return Result; }

        WriteableNodeStateView IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>.this[IWriteableNodeState key] { get { return (WriteableNodeStateView)this[key]; } }
        IEnumerable<IWriteableNodeState> IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>.Keys { get { List<IWriteableNodeState> Result = new(); foreach (KeyValuePair<IWriteableNodeState, WriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<WriteableNodeStateView> IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>.Values { get { List<WriteableNodeStateView> Result = new(); foreach (KeyValuePair<IWriteableNodeState, WriteableNodeStateView> Entry in (ICollection<KeyValuePair<IWriteableNodeState, WriteableNodeStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IWriteableNodeState, WriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out WriteableNodeStateView value) { bool Result = TryGetValue(key, out ReadOnlyNodeStateView Value); value = (WriteableNodeStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IWriteableNodeState Key in Keys)
            {
                WriteableNodeStateView Value = (WriteableNodeStateView)this[Key];

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
            return new WriteableStateViewReadOnlyDictionary(this);
        }
    }
}
