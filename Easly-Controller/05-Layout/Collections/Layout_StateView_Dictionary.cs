#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface ILayoutStateViewDictionary : IFocusStateViewDictionary, IDictionary<ILayoutNodeState, ILayoutNodeStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<ILayoutNodeState, ILayoutNodeStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class LayoutStateViewDictionary : Dictionary<ILayoutNodeState, ILayoutNodeStateView>, ILayoutStateViewDictionary
    {
        #region ReadOnly
        IReadOnlyNodeStateView IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.this[IReadOnlyNodeState key] { get { return this[(ILayoutNodeState)key]; } set { this[(ILayoutNodeState)key] = (ILayoutNodeStateView)value; } }
        void IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Add(IReadOnlyNodeState key, IReadOnlyNodeStateView value) { Add((ILayoutNodeState)key, (ILayoutNodeStateView)value); }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.ContainsKey(IReadOnlyNodeState key) { return ContainsKey((ILayoutNodeState)key); }
        ICollection<IReadOnlyNodeState> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Keys { get { return new List<IReadOnlyNodeState>(Keys); } }
        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Remove(IReadOnlyNodeState key) { return Remove((ILayoutNodeState)key); }

        bool IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.TryGetValue(IReadOnlyNodeState key, out IReadOnlyNodeStateView value)
        {
            bool Result = TryGetValue((ILayoutNodeState)key, out ILayoutNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyNodeStateView> IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>.Values { get { return new List<IReadOnlyNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Add(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { Add((ILayoutNodeState)item.Key, (ILayoutNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Contains(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return ContainsKey((ILayoutNodeState)item.Key) && this[(ILayoutNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.CopyTo(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.Remove(KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> item) { return Remove((ILayoutNodeState)item.Key); }

        IEnumerator<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> IEnumerable<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> NewList = new List<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        Dictionary<IWriteableNodeState, IWriteableNodeStateView>.Enumerator IWriteableStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableNodeState, IWriteableNodeStateView> NewDictionary = new Dictionary<IWriteableNodeState, IWriteableNodeStateView>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IWriteableNodeStateView IDictionary<IWriteableNodeState, IWriteableNodeStateView>.this[IWriteableNodeState key] { get { return this[(ILayoutNodeState)key]; } set { this[(ILayoutNodeState)key] = (ILayoutNodeStateView)value; } }
        void IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Add(IWriteableNodeState key, IWriteableNodeStateView value) { Add((ILayoutNodeState)key, (ILayoutNodeStateView)value); }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.ContainsKey(IWriteableNodeState key) { return ContainsKey((ILayoutNodeState)key); }
        ICollection<IWriteableNodeState> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Keys { get { return new List<IWriteableNodeState>(Keys); } }
        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Remove(IWriteableNodeState key) { return Remove((ILayoutNodeState)key); }

        bool IDictionary<IWriteableNodeState, IWriteableNodeStateView>.TryGetValue(IWriteableNodeState key, out IWriteableNodeStateView value)
        {
            bool Result = TryGetValue((ILayoutNodeState)key, out ILayoutNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableNodeStateView> IDictionary<IWriteableNodeState, IWriteableNodeStateView>.Values { get { return new List<IWriteableNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Add(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { Add((ILayoutNodeState)item.Key, (ILayoutNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Contains(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return ContainsKey((ILayoutNodeState)item.Key) && this[(ILayoutNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.CopyTo(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.Remove(KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> item) { return Remove((ILayoutNodeState)item.Key); }

        IEnumerator<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> NewList = new List<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Frame
        Dictionary<IFrameNodeState, IFrameNodeStateView>.Enumerator IFrameStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFrameNodeState, IFrameNodeStateView> NewDictionary = new Dictionary<IFrameNodeState, IFrameNodeStateView>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFrameNodeStateView IDictionary<IFrameNodeState, IFrameNodeStateView>.this[IFrameNodeState key] { get { return this[(ILayoutNodeState)key]; } set { this[(ILayoutNodeState)key] = (ILayoutNodeStateView)value; } }
        void IDictionary<IFrameNodeState, IFrameNodeStateView>.Add(IFrameNodeState key, IFrameNodeStateView value) { Add((ILayoutNodeState)key, (ILayoutNodeStateView)value); }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.ContainsKey(IFrameNodeState key) { return ContainsKey((ILayoutNodeState)key); }
        ICollection<IFrameNodeState> IDictionary<IFrameNodeState, IFrameNodeStateView>.Keys { get { return new List<IFrameNodeState>(Keys); } }
        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.Remove(IFrameNodeState key) { return Remove((ILayoutNodeState)key); }

        bool IDictionary<IFrameNodeState, IFrameNodeStateView>.TryGetValue(IFrameNodeState key, out IFrameNodeStateView value)
        {
            bool Result = TryGetValue((ILayoutNodeState)key, out ILayoutNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameNodeStateView> IDictionary<IFrameNodeState, IFrameNodeStateView>.Values { get { return new List<IFrameNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Add(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { Add((ILayoutNodeState)item.Key, (ILayoutNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Contains(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return ContainsKey((ILayoutNodeState)item.Key) && this[(ILayoutNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.CopyTo(KeyValuePair<IFrameNodeState, IFrameNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFrameNodeState, IFrameNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.Remove(KeyValuePair<IFrameNodeState, IFrameNodeStateView> item) { return Remove((ILayoutNodeState)item.Key); }

        IEnumerator<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> NewList = new List<KeyValuePair<IFrameNodeState, IFrameNodeStateView>>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameNodeState, IFrameNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        Dictionary<IFocusNodeState, IFocusNodeStateView>.Enumerator IFocusStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFocusNodeState, IFocusNodeStateView> NewDictionary = new Dictionary<IFocusNodeState, IFocusNodeStateView>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFocusNodeStateView IDictionary<IFocusNodeState, IFocusNodeStateView>.this[IFocusNodeState key] { get { return this[(ILayoutNodeState)key]; } set { this[(ILayoutNodeState)key] = (ILayoutNodeStateView)value; } }
        void IDictionary<IFocusNodeState, IFocusNodeStateView>.Add(IFocusNodeState key, IFocusNodeStateView value) { Add((ILayoutNodeState)key, (ILayoutNodeStateView)value); }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.ContainsKey(IFocusNodeState key) { return ContainsKey((ILayoutNodeState)key); }
        ICollection<IFocusNodeState> IDictionary<IFocusNodeState, IFocusNodeStateView>.Keys { get { return new List<IFocusNodeState>(Keys); } }
        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.Remove(IFocusNodeState key) { return Remove((ILayoutNodeState)key); }

        bool IDictionary<IFocusNodeState, IFocusNodeStateView>.TryGetValue(IFocusNodeState key, out IFocusNodeStateView value)
        {
            bool Result = TryGetValue((ILayoutNodeState)key, out ILayoutNodeStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IFocusNodeStateView> IDictionary<IFocusNodeState, IFocusNodeStateView>.Values { get { return new List<IFocusNodeStateView>(Values); } }
        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Add(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { Add((ILayoutNodeState)item.Key, (ILayoutNodeStateView)item.Value); }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Contains(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { return ContainsKey((ILayoutNodeState)item.Key) && this[(ILayoutNodeState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.CopyTo(KeyValuePair<IFocusNodeState, IFocusNodeStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFocusNodeState, IFocusNodeStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.Remove(KeyValuePair<IFocusNodeState, IFocusNodeStateView> item) { return Remove((ILayoutNodeState)item.Key); }

        IEnumerator<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> NewList = new List<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>();
            IEnumerator<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFocusNodeState, IFocusNodeStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in this)
            {
                ILayoutNodeState Key = Entry.Key;
                ILayoutNodeStateView Value = Entry.Value;

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
