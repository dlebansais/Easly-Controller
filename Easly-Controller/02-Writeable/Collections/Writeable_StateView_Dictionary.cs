using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IWriteableStateViewDictionary : IReadOnlyStateViewDictionary, IDictionary<IWriteableNodeState, IWriteableNodeStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IWriteableNodeState, IWriteableNodeStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class WriteableStateViewDictionary : Dictionary<IWriteableNodeState, IWriteableNodeStateView>, IWriteableStateViewDictionary
    {
        void IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Add(IReadOnlyNodeState key, IReadOnlyNodeStateView value) { Add((IWriteableNodeState)key, (IWriteableNodeStateView)value); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Remove(IReadOnlyNodeState key) { return Remove((IWriteableNodeState)key); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.TryGetValue(IReadOnlyNodeState key, out IReadOnlyNodeStateView value) { bool Result = TryGetValue((IWriteableNodeState)key, out IWriteableNodeStateView Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.ContainsKey(IReadOnlyNodeState key) { return ContainsKey((IWriteableNodeState)key); }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Add(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { Add((IWriteableNodeState)item.Key, (IWriteableNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Contains(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return ContainsKey((IWriteableNodeState)item.Key) && base[(IWriteableNodeState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.CopyTo(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Remove(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return Remove((IWriteableNodeState)item.Key); }
        IReadOnlyNodeStateView IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.this[IReadOnlyNodeState key] { get { return this[(IWriteableNodeState)key]; } set { this[(IWriteableNodeState)key] = (IWriteableNodeStateView)value; } }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Keys { get { return new List<IReadOnlyNodeState>(Keys); } }
        ICollection<IReadOnlyNodeStateView> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Values { get { return new List<IReadOnlyNodeStateView>(Values); } }
        public void Add(IWriteableNodeState key, IReadOnlyNodeStateView value) { base.Add(key, (IWriteableNodeStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> IEnumerable<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> NewList = new List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>();
            IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IWriteableNodeState key, out IReadOnlyNodeStateView value) { bool Result = TryGetValue(key, out IWriteableNodeStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IWriteableNodeState, IReadOnlyNodeStateView> item) { base.Add(item.Key, (IWriteableNodeStateView)item.Value); }
        public bool Contains(KeyValuePair<IWriteableNodeState, IReadOnlyNodeStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IWriteableNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IWriteableNodeState, IReadOnlyNodeStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>)this).IsReadOnly; } }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableStateViewDictionary AsStateViewDictionary))
                return false;

            if (Count != AsStateViewDictionary.Count)
                return false;

            foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in this)
            {
                IWriteableNodeState Key = Entry.Key;
                IWriteableNodeStateView Value = Entry.Value;

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
