#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFrameIndexNodeStateDictionary : IWriteableIndexNodeStateDictionary, IDictionary<IFrameIndex, IFrameNodeState>
    {
        new int Count { get; }
        new Dictionary<IFrameIndex, IFrameNodeState>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class FrameIndexNodeStateDictionary : Dictionary<IFrameIndex, IFrameNodeState>, IFrameIndexNodeStateDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Add(IReadOnlyIndex key, IReadOnlyNodeState value) { Add((IFrameIndex)key, (IFrameNodeState)value); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Remove(IReadOnlyIndex key) { return Remove((IFrameIndex)key); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value)
        {
            bool Result = TryGetValue((IFrameIndex)key, out IFrameNodeState Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IFrameIndex)key); }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Add(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { Add((IFrameIndex)item.Key, (IFrameNodeState)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Contains(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return ContainsKey((IFrameIndex)item.Key) && base[(IFrameIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.CopyTo(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Remove(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return Remove((IFrameIndex)item.Key); }
        IReadOnlyNodeState IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IFrameIndex)key]; } set { this[(IFrameIndex)key] = (IFrameNodeState)value; } }
        ICollection<IReadOnlyIndex> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }
        public void Add(IFrameIndex key, IReadOnlyNodeState value) { base.Add(key, (IFrameNodeState)value); }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameIndex, IFrameNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { Add((IFrameIndex)key, (IFrameNodeState)value); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { return Remove((IFrameIndex)key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value)
        {
            bool Result = TryGetValue((IFrameIndex)key, out IFrameNodeState Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((IFrameIndex)key); }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { Add((IFrameIndex)item.Key, (IFrameNodeState)item.Value); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey((IFrameIndex)item.Key) && base[(IFrameIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return Remove((IFrameIndex)item.Key); }
        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(IFrameIndex)key]; } set { this[(IFrameIndex)key] = (IFrameNodeState)value; } }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }
        public void Add(IFrameIndex key, IWriteableNodeState value) { base.Add(key, (IFrameNodeState)value); }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameIndex, IFrameNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableIndex, IWriteableNodeState>.Enumerator IWriteableIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IWriteableIndex, IWriteableNodeState> NewDictionary = new Dictionary<IWriteableIndex, IWriteableNodeState>();
            IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameIndex, IFrameNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)this).IsReadOnly; } }
        #endregion
    }
}
