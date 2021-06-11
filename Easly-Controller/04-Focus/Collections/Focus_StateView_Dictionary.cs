#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

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
    internal class FocusStateViewDictionary : Dictionary<IFocusNodeState, IFocusNodeStateView>, IFocusStateViewDictionary
    {
        #region ReadOnly
        IReadOnlyNodeStateView IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.this[IReadOnlyNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        void IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Add(IReadOnlyNodeState key, IReadOnlyNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.ContainsKey(IReadOnlyNodeState key) { return ContainsKey((IFocusNodeState)key); }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Keys { get { return new List<IReadOnlyNodeState>(Keys); } }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Remove(IReadOnlyNodeState key) { return Remove((IFocusNodeState)key); }

        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.TryGetValue(IReadOnlyNodeState key, out IReadOnlyNodeStateView value)
        {
            bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyNodeStateView> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Values { get { return new List<IReadOnlyNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Add(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Contains(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && this[(IFocusNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.CopyTo(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Remove(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }

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
        #endregion

        #region Writeable
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

        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey((IFocusNodeState)key); }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { return new List<IWriteableNodeState>(Keys); } }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove((IFocusNodeState)key); }

        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value)
        {
            bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { return new List<IWriteableNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && this[(IFocusNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }

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
        #endregion

        #region Frame
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

        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return this[(IFocusNodeState)key]; } set { this[(IFocusNodeState)key] = (IFocusNodeStateView)value; } }
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { Add((IFocusNodeState)key, (IFocusNodeStateView)value); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey((IFocusNodeState)key); }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { return new List<IFrameNodeState>(Keys); } }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { return Remove((IFocusNodeState)key); }

        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value)
        {
            bool Result = TryGetValue((IFocusNodeState)key, out IFocusNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { return new List<IFrameNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { Add((IFocusNodeState)item.Key, (IFocusNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey((IFocusNodeState)item.Key) && this[(IFocusNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFrameNodeState, IFrameNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return Remove((IFocusNodeState)item.Key); }

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
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in this)
            {
                IFocusNodeState Key = Entry.Key;
                IFocusNodeStateView Value = Entry.Value;

                if (!comparer.IsTrue(AsStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
