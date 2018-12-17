using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IWriteableIndexNodeStateDictionary : IReadOnlyIndexNodeStateDictionary, IDictionary<IWriteableIndex, IWriteableNodeState>
    {
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class WriteableIndexNodeStateDictionary : Dictionary<IWriteableIndex, IWriteableNodeState>, IWriteableIndexNodeStateDictionary
    {
        void IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Add(IReadOnlyIndex key, IReadOnlyNodeState value) { Add((IWriteableIndex)key, (IWriteableNodeState)value); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Remove(IReadOnlyIndex key) { return Remove((IWriteableIndex)key); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue((IWriteableIndex)key, out IWriteableNodeState Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IWriteableIndex)key); }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Add(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { Add((IWriteableIndex)item.Key, (IWriteableNodeState)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Contains(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return ContainsKey((IWriteableIndex)item.Key) && base[(IWriteableIndex)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.CopyTo(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Remove(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return Remove((IWriteableIndex)item.Key); }
        IReadOnlyNodeState IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IWriteableIndex)key]; } set { this[(IWriteableIndex)key] = (IWriteableNodeState)value; } }
        ICollection<IReadOnlyIndex> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }
        public void Add(IWriteableIndex key, IReadOnlyNodeState value) { base.Add(key, (IWriteableNodeState)value); }

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

        public bool TryGetValue(IWriteableIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue(key, out IWriteableNodeState Value); value = Value; return Result; }
        public void Add(KeyValuePair<IWriteableIndex, IReadOnlyNodeState> item) { base.Add(item.Key, (IWriteableNodeState)item.Value); }
        public bool Contains(KeyValuePair<IWriteableIndex, IReadOnlyNodeState> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IWriteableIndex, IReadOnlyNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IWriteableIndex, IReadOnlyNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)this).IsReadOnly; } }
    }
}
