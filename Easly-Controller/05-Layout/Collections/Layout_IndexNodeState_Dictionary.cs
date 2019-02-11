#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface ILayoutIndexNodeStateDictionary : IFocusIndexNodeStateDictionary, IDictionary<ILayoutIndex, ILayoutNodeState>
    {
        new int Count { get; }
        new Dictionary<ILayoutIndex, ILayoutNodeState>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class LayoutIndexNodeStateDictionary : Dictionary<ILayoutIndex, ILayoutNodeState>, ILayoutIndexNodeStateDictionary
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IReadOnlyIndexNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new LayoutIndexNodeStateReadOnlyDictionary(this);
        }

        #region ReadOnly
        IReadOnlyNodeState IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(ILayoutIndex)key]; } set { this[(ILayoutIndex)key] = (ILayoutNodeState)value; } }
        void IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Add(IReadOnlyIndex key, IReadOnlyNodeState value) { Add((ILayoutIndex)key, (ILayoutNodeState)value); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((ILayoutIndex)key); }
        ICollection<IReadOnlyIndex> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Remove(IReadOnlyIndex key) { return Remove((ILayoutIndex)key); }

        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Add(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { Add((ILayoutIndex)item.Key, (ILayoutNodeState)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Contains(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return ContainsKey((ILayoutIndex)item.Key) && this[(ILayoutIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.CopyTo(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Remove(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return Remove((ILayoutIndex)item.Key); }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        Dictionary<IWriteableIndex, IWriteableNodeState>.Enumerator IWriteableIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IWriteableIndex, IWriteableNodeState> NewDictionary = new Dictionary<IWriteableIndex, IWriteableNodeState>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(ILayoutIndex)key]; } set { this[(ILayoutIndex)key] = (ILayoutNodeState)value; } }
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { Add((ILayoutIndex)key, (ILayoutNodeState)value); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((ILayoutIndex)key); }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { return Remove((ILayoutIndex)key); }

        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { Add((ILayoutIndex)item.Key, (ILayoutNodeState)item.Value); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey((ILayoutIndex)item.Key) && this[(ILayoutIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return Remove((ILayoutIndex)item.Key); }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Frame
        Dictionary<IFrameIndex, IFrameNodeState>.Enumerator IFrameIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IFrameIndex, IFrameNodeState> NewDictionary = new Dictionary<IFrameIndex, IFrameNodeState>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFrameNodeState IDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return this[(ILayoutIndex)key]; } set { this[(ILayoutIndex)key] = (ILayoutNodeState)value; } }
        void IDictionary<IFrameIndex, IFrameNodeState>.Add(IFrameIndex key, IFrameNodeState value) { Add((ILayoutIndex)key, (ILayoutNodeState)value); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey((ILayoutIndex)key); }
        ICollection<IFrameIndex> IDictionary<IFrameIndex, IFrameNodeState>.Keys { get { return new List<IFrameIndex>(Keys); } }
        bool IDictionary<IFrameIndex, IFrameNodeState>.Remove(IFrameIndex key) { return Remove((ILayoutIndex)key); }

        bool IDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameNodeState> IDictionary<IFrameIndex, IFrameNodeState>.Values { get { return new List<IFrameNodeState>(Values); } }
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Add(KeyValuePair<IFrameIndex, IFrameNodeState> item) { Add((ILayoutIndex)item.Key, (ILayoutNodeState)item.Value); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Contains(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return ContainsKey((ILayoutIndex)item.Key) && this[(ILayoutIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.CopyTo(KeyValuePair<IFrameIndex, IFrameNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFrameIndex, IFrameNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Remove(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return Remove((ILayoutIndex)item.Key); }

        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameIndex, IFrameNodeState>> NewList = new List<KeyValuePair<IFrameIndex, IFrameNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameIndex, IFrameNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        Dictionary<IFocusIndex, IFocusNodeState>.Enumerator IFocusIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IFocusIndex, IFocusNodeState> NewDictionary = new Dictionary<IFocusIndex, IFocusNodeState>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFocusNodeState IDictionary<IFocusIndex, IFocusNodeState>.this[IFocusIndex key] { get { return this[(ILayoutIndex)key]; } set { this[(ILayoutIndex)key] = (ILayoutNodeState)value; } }
        void IDictionary<IFocusIndex, IFocusNodeState>.Add(IFocusIndex key, IFocusNodeState value) { Add((ILayoutIndex)key, (ILayoutNodeState)value); }
        bool IDictionary<IFocusIndex, IFocusNodeState>.ContainsKey(IFocusIndex key) { return ContainsKey((ILayoutIndex)key); }
        ICollection<IFocusIndex> IDictionary<IFocusIndex, IFocusNodeState>.Keys { get { return new List<IFocusIndex>(Keys); } }
        bool IDictionary<IFocusIndex, IFocusNodeState>.Remove(IFocusIndex key) { return Remove((ILayoutIndex)key); }

        bool IDictionary<IFocusIndex, IFocusNodeState>.TryGetValue(IFocusIndex key, out IFocusNodeState value)
        {
            bool Result = TryGetValue((ILayoutIndex)key, out ILayoutNodeState Value);
            value = Value;
            return Result;
        }

        ICollection<IFocusNodeState> IDictionary<IFocusIndex, IFocusNodeState>.Values { get { return new List<IFocusNodeState>(Values); } }
        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Add(KeyValuePair<IFocusIndex, IFocusNodeState> item) { Add((ILayoutIndex)item.Key, (ILayoutNodeState)item.Value); }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Contains(KeyValuePair<IFocusIndex, IFocusNodeState> item) { return ContainsKey((ILayoutIndex)item.Key) && this[(ILayoutIndex)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.CopyTo(KeyValuePair<IFocusIndex, IFocusNodeState>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFocusIndex, IFocusNodeState>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutIndex, ILayoutNodeState>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>.Remove(KeyValuePair<IFocusIndex, IFocusNodeState> item) { return Remove((ILayoutIndex)item.Key); }

        IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFocusIndex, IFocusNodeState>> NewList = new List<KeyValuePair<IFocusIndex, IFocusNodeState>>();
            IEnumerator<KeyValuePair<ILayoutIndex, ILayoutNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutIndex, ILayoutNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFocusIndex, IFocusNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion
    }
}
