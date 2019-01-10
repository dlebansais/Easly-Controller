using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFocusStateViewDictionary : IFrameStateViewDictionary, IDictionary<IFocusNodeState, IFocusNodeStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IFocusNodeState, IFocusNodeStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class FocusStateViewDictionary : Dictionary<IFocusNodeState, IFocusNodeStateView>, IFocusStateViewDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Add(IReadOnlyNodeState key, IReadOnlyNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Remove(IReadOnlyNodeState key) { return Remove((IFocusNodeState)key); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.TryGetValue(IReadOnlyNodeState key, out IReadOnlyNodeStateView value) { bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.ContainsKey(IReadOnlyNodeState key) { return ContainsKey((IFocusNodeState)key); }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Add(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Contains(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && base[(IFocusNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.CopyTo(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Remove(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }
        IReadOnlyNodeStateView IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.this[IReadOnlyNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Keys { get { return new List<IReadOnlyNodeState>(Keys); } }
        ICollection<IReadOnlyNodeStateView> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Values { get { return new List<IReadOnlyNodeStateView>(Values); } }
        public void Add(IFocusNodeState key, IReadOnlyNodeStateView value) { base.Add(key, (IFocusNodeStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> IEnumerable<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> NewList = new List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>();
            IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IFocusNodeState key, out IReadOnlyNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusNodeState, IReadOnlyNodeStateView> item) { base.Add(item.Key, (IFocusNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusNodeState, IReadOnlyNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusNodeState, IReadOnlyNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove((IFocusNodeState)key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value); value = Value; return Result; }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey((IFocusNodeState)key); }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && base[(IFocusNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }
        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { return new List<IWriteableNodeState>(Keys); } }
        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { return new List<IWriteableNodeStateView>(Values); } }
        public void Add(IFocusNodeState key, IWriteableNodeStateView value) { base.Add(key, (IFocusNodeStateView)value); }

        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> NewList = new List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>();
            IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableNodeState, IWriteableNodeStateView>.Enumerator IWriteableStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableNodeState, IWriteableNodeStateView> NewDictionary = new Dictionary<IWriteableNodeState, IWriteableNodeStateView>();
            IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusNodeState key, out IWriteableNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusNodeState, IWriteableNodeStateView> item) { base.Add(item.Key, (IFocusNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusNodeState, IWriteableNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusNodeState, IWriteableNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        #endregion

        #region Frame
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { return Remove((IFocusNodeState)key); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value); value = Value; return Result; }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey((IFocusNodeState)key); }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && base[(IFocusNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }
        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { return new List<IFrameNodeState>(Keys); } }
        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { return new List<IFrameNodeStateView>(Values); } }
        public void Add(IFocusNodeState key, IFrameNodeStateView value) { base.Add(key, (IFocusNodeStateView)value); }

        IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> NewList = new List<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>();
            IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameNodeState, IFrameNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IFrameNodeState, IFrameNodeStateView>.Enumerator IFrameStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFrameNodeState, IFrameNodeStateView> NewDictionary = new Dictionary<IFrameNodeState, IFrameNodeStateView>();
            IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusNodeState key, out IFrameNodeStateView value) { bool Result = TryGetValue(key, out IFocusNodeStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFocusNodeState, IFrameNodeStateView> item) { base.Add(item.Key, (IFocusNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusNodeState, IFrameNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusNodeState, IFrameNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusNodeState, IFrameNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusStateViewDictionary AsStateViewDictionary))
                return false;

            if (Count != AsStateViewDictionary.Count)
                return false;

            foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in this)
            {
                IFocusNodeState Key = Entry.Key;
                IFocusNodeStateView Value = Entry.Value;

                if (!AsStateViewDictionary.ContainsKey(Key))
                    return false;

                if (!comparer.VerifyEqual(Value, AsStateViewDictionary[Key]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
