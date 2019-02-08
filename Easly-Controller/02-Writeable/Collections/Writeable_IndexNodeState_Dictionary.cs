#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IWriteableIndexNodeStateDictionary : IReadOnlyIndexNodeStateDictionary, IDictionary<IWriteableIndex, IWriteableNodeState>
    {
        new int Count { get; }
        new Dictionary<IWriteableIndex, IWriteableNodeState>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class WriteableIndexNodeStateDictionary : Dictionary<IWriteableIndex, IWriteableNodeState>, IWriteableIndexNodeStateDictionary
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new WriteableIndexNodeStateReadOnlyDictionary(this);
        }

        #region ReadOnly
        IReadOnlyNodeState IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IWriteableIndex)key]; } set { this[(IWriteableIndex)key] = (IWriteableNodeState)value; } }
        void IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Add(IReadOnlyIndex key, IReadOnlyNodeState value) { Add((IWriteableIndex)key, (IWriteableNodeState)value); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IWriteableIndex)key); }
        ICollection<IReadOnlyIndex> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Remove(IReadOnlyIndex key) { return Remove((IWriteableIndex)key); }

        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value)
        {
            bool Result = TryGetValue((IWriteableIndex)key, out IWriteableNodeState Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Add(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { Add((IWriteableIndex)item.Key, (IWriteableNodeState)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Contains(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return ContainsKey((IWriteableIndex)item.Key) && this[(IWriteableIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.CopyTo(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Remove(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return Remove((IWriteableIndex)item.Key); }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
