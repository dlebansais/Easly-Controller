﻿using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFocusIndexNodeStateDictionary : IFrameIndexNodeStateDictionary, IDictionary<IFocusIndex, IFocusNodeState>
    {
        new int Count { get; }
        new Dictionary<IFocusIndex, IFocusNodeState>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class FocusIndexNodeStateDictionary : Dictionary<IFocusIndex, IFocusNodeState>, IFocusIndexNodeStateDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Add(IReadOnlyIndex key, IReadOnlyNodeState value) { Add((IFocusIndex)key, (IFocusNodeState)value); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Remove(IReadOnlyIndex key) { return Remove((IFocusIndex)key); }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.TryGetValue(IReadOnlyIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.ContainsKey(IReadOnlyIndex key) { return ContainsKey((IFocusIndex)key); }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Add(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { Add((IFocusIndex)item.Key, (IFocusNodeState)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Contains(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return ContainsKey((IFocusIndex)item.Key) && base[(IFocusIndex)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.CopyTo(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.Remove(KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> item) { return Remove((IFocusIndex)item.Key); }
        IReadOnlyNodeState IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.this[IReadOnlyIndex key] { get { return this[(IFocusIndex)key]; } set { this[(IFocusIndex)key] = (IFocusNodeState)value; } }
        ICollection<IReadOnlyIndex> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Keys { get { return new List<IReadOnlyIndex>(Keys); } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyIndex, IReadOnlyNodeState>.Values { get { return new List<IReadOnlyNodeState>(Values); } }
        public void Add(IFocusIndex key, IReadOnlyNodeState value) { base.Add(key, (IFocusNodeState)value); }

        IEnumerator<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> NewList = new List<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IFocusIndex key, out IReadOnlyNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusIndex, IReadOnlyNodeState> item) { base.Add(item.Key, (IFocusNodeState)item.Value); }
        public bool Contains(KeyValuePair<IFocusIndex, IReadOnlyNodeState> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusIndex, IReadOnlyNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusIndex, IReadOnlyNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableIndex, IWriteableNodeState>.Add(IWriteableIndex key, IWriteableNodeState value) { Add((IFocusIndex)key, (IFocusNodeState)value); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.Remove(IWriteableIndex key) { return Remove((IFocusIndex)key); }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.TryGetValue(IWriteableIndex key, out IWriteableNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        bool IDictionary<IWriteableIndex, IWriteableNodeState>.ContainsKey(IWriteableIndex key) { return ContainsKey((IFocusIndex)key); }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Add(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { Add((IFocusIndex)item.Key, (IFocusNodeState)item.Value); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Contains(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return ContainsKey((IFocusIndex)item.Key) && base[(IFocusIndex)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.CopyTo(KeyValuePair<IWriteableIndex, IWriteableNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.Remove(KeyValuePair<IWriteableIndex, IWriteableNodeState> item) { return Remove((IFocusIndex)item.Key); }
        IWriteableNodeState IDictionary<IWriteableIndex, IWriteableNodeState>.this[IWriteableIndex key] { get { return this[(IFocusIndex)key]; } set { this[(IFocusIndex)key] = (IFocusNodeState)value; } }
        ICollection<IWriteableIndex> IDictionary<IWriteableIndex, IWriteableNodeState>.Keys { get { return new List<IWriteableIndex>(Keys); } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableIndex, IWriteableNodeState>.Values { get { return new List<IWriteableNodeState>(Values); } }
        public void Add(IFocusIndex key, IWriteableNodeState value) { base.Add(key, (IFocusNodeState)value); }

        IEnumerator<KeyValuePair<IWriteableIndex, IWriteableNodeState>> IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableIndex, IWriteableNodeState>> NewList = new List<KeyValuePair<IWriteableIndex, IWriteableNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableIndex, IWriteableNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableIndex, IWriteableNodeState>.Enumerator IWriteableIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IWriteableIndex, IWriteableNodeState> NewDictionary = new Dictionary<IWriteableIndex, IWriteableNodeState>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusIndex key, out IWriteableNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusIndex, IWriteableNodeState> item) { base.Add(item.Key, (IFocusNodeState)item.Value); }
        public bool Contains(KeyValuePair<IFocusIndex, IWriteableNodeState> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusIndex, IWriteableNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusIndex, IWriteableNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).IsReadOnly; } }
        #endregion

        #region Frame
        void IDictionary<IFrameIndex, IFrameNodeState>.Add(IFrameIndex key, IFrameNodeState value) { Add((IFocusIndex)key, (IFocusNodeState)value); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.Remove(IFrameIndex key) { return Remove((IFocusIndex)key); }
        bool IDictionary<IFrameIndex, IFrameNodeState>.TryGetValue(IFrameIndex key, out IFrameNodeState value) { bool Result = TryGetValue((IFocusIndex)key, out IFocusNodeState Value); value = Value; return Result; }
        bool IDictionary<IFrameIndex, IFrameNodeState>.ContainsKey(IFrameIndex key) { return ContainsKey((IFocusIndex)key); }
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Add(KeyValuePair<IFrameIndex, IFrameNodeState> item) { Add((IFocusIndex)item.Key, (IFocusNodeState)item.Value); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Contains(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return ContainsKey((IFocusIndex)item.Key) && base[(IFocusIndex)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.CopyTo(KeyValuePair<IFrameIndex, IFrameNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.Remove(KeyValuePair<IFrameIndex, IFrameNodeState> item) { return Remove((IFocusIndex)item.Key); }
        IFrameNodeState IDictionary<IFrameIndex, IFrameNodeState>.this[IFrameIndex key] { get { return this[(IFocusIndex)key]; } set { this[(IFocusIndex)key] = (IFocusNodeState)value; } }
        ICollection<IFrameIndex> IDictionary<IFrameIndex, IFrameNodeState>.Keys { get { return new List<IFrameIndex>(Keys); } }
        ICollection<IFrameNodeState> IDictionary<IFrameIndex, IFrameNodeState>.Values { get { return new List<IFrameNodeState>(Values); } }
        public void Add(IFocusIndex key, IFrameNodeState value) { base.Add(key, (IFocusNodeState)value); }

        IEnumerator<KeyValuePair<IFrameIndex, IFrameNodeState>> IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameIndex, IFrameNodeState>> NewList = new List<KeyValuePair<IFrameIndex, IFrameNodeState>>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameIndex, IFrameNodeState>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IFrameIndex, IFrameNodeState>.Enumerator IFrameIndexNodeStateDictionary.GetEnumerator()
        {
            Dictionary<IFrameIndex, IFrameNodeState> NewDictionary = new Dictionary<IFrameIndex, IFrameNodeState>();
            IEnumerator<KeyValuePair<IFocusIndex, IFocusNodeState>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusIndex, IFocusNodeState> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusIndex key, out IFrameNodeState value) { bool Result = TryGetValue(key, out IFocusNodeState Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusIndex, IFrameNodeState> item) { base.Add(item.Key, (IFocusNodeState)item.Value); }
        public bool Contains(KeyValuePair<IFocusIndex, IFrameNodeState> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusIndex, IFrameNodeState>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusIndex, IFrameNodeState> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)this).IsReadOnly; } }
        #endregion
    }
}