#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IFrameStateViewDictionary : IWriteableStateViewDictionary, IDictionary<IFrameNodeState, IFrameNodeStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IFrameNodeState, IFrameNodeStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class FrameStateViewDictionary : Dictionary<IFrameNodeState, IFrameNodeStateView>, IFrameStateViewDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Add(IReadOnlyNodeState key, IReadOnlyNodeStateView value) { Add((IFrameNodeState)key, (IFrameNodeStateView)value); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Remove(IReadOnlyNodeState key) { return Remove((IFrameNodeState)key); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.TryGetValue(IReadOnlyNodeState key, out IReadOnlyNodeStateView value)
        {
            bool Result = TryGetValue((IFrameNodeState)key, out IFrameNodeStateView Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.ContainsKey(IReadOnlyNodeState key) { return ContainsKey((IFrameNodeState)key); }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Add(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { Add((IFrameNodeState)item.Key, (IFrameNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Contains(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return ContainsKey((IFrameNodeState)item.Key) && base[(IFrameNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.CopyTo(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Remove(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return Remove((IFrameNodeState)item.Key); }
        IReadOnlyNodeStateView IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.this[IReadOnlyNodeState key] { get { return this[(IFrameNodeState)key]; } set { this[(IFrameNodeState)key] = (IFrameNodeStateView)value; } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Keys { get { return new List<IReadOnlyNodeState>(Keys); } }
        ICollection<IReadOnlyNodeStateView> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Values { get { return new List<IReadOnlyNodeStateView>(Values); } }
        public void Add(IFrameNodeState key, IReadOnlyNodeStateView value) { base.Add(key, (IFrameNodeStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> IEnumerable<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> NewList = new List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>();
            IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IFrameNodeState key, out IReadOnlyNodeStateView value)
        {
            bool Result = TryGetValue(key, out IFrameNodeStateView Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<IFrameNodeState, IReadOnlyNodeStateView> item) { base.Add(item.Key, (IFrameNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IFrameNodeState, IReadOnlyNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFrameNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFrameNodeState, IReadOnlyNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { Add((IFrameNodeState)key, (IFrameNodeStateView)value); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove((IFrameNodeState)key); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value)
        {
            bool Result = TryGetValue((IFrameNodeState)key, out IFrameNodeStateView Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey((IFrameNodeState)key); }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { Add((IFrameNodeState)item.Key, (IFrameNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey((IFrameNodeState)item.Key) && base[(IFrameNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return Remove((IFrameNodeState)item.Key); }
        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return this[(IFrameNodeState)key]; } set { this[(IFrameNodeState)key] = (IFrameNodeStateView)value; } }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { return new List<IWriteableNodeState>(Keys); } }
        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { return new List<IWriteableNodeStateView>(Values); } }
        public void Add(IFrameNodeState key, IWriteableNodeStateView value) { base.Add(key, (IFrameNodeStateView)value); }

        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> NewList = new List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>();
            IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableNodeState, IWriteableNodeStateView>.Enumerator IWriteableStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableNodeState, IWriteableNodeStateView> NewDictionary = new Dictionary<IWriteableNodeState, IWriteableNodeStateView>();
            IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFrameNodeState key, out IWriteableNodeStateView value)
        {
            bool Result = TryGetValue(key, out IFrameNodeStateView Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<IFrameNodeState, IWriteableNodeStateView> item) { base.Add(item.Key, (IFrameNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IFrameNodeState, IWriteableNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFrameNodeState, IWriteableNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFrameNodeState, IWriteableNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>)this).IsReadOnly; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (Count != AsStateViewDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in this)
            {
                IFrameNodeState Key = Entry.Key;
                IFrameNodeStateView Value = Entry.Value;

                if (!AsStateViewDictionary.ContainsKey(Key))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
