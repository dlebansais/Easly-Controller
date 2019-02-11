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
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public interface ILayoutBlockStateViewDictionary : IFocusBlockStateViewDictionary, IDictionary<ILayoutBlockState, ILayoutBlockStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<ILayoutBlockState, ILayoutBlockStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    internal class LayoutBlockStateViewDictionary : Dictionary<ILayoutBlockState, ILayoutBlockStateView>, ILayoutBlockStateViewDictionary
    {
        #region ReadOnly
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(ILayoutBlockState)key]; } set { this[(ILayoutBlockState)key] = (ILayoutBlockStateView)value; } }
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((ILayoutBlockState)key, (ILayoutBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((ILayoutBlockState)key); }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((ILayoutBlockState)key); }

        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value)
        {
            bool Result = TryGetValue((ILayoutBlockState)key, out ILayoutBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((ILayoutBlockState)item.Key, (ILayoutBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((ILayoutBlockState)item.Key) && this[(ILayoutBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((ILayoutBlockState)item.Key); }

        IEnumerator<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> IEnumerable<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> NewList = new List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Writeable
        Dictionary<IWriteableBlockState, IWriteableBlockStateView>.Enumerator IWriteableBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableBlockState, IWriteableBlockStateView> NewDictionary = new Dictionary<IWriteableBlockState, IWriteableBlockStateView>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return this[(ILayoutBlockState)key]; } set { this[(ILayoutBlockState)key] = (ILayoutBlockStateView)value; } }
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { Add((ILayoutBlockState)key, (ILayoutBlockStateView)value); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey((ILayoutBlockState)key); }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { return new List<IWriteableBlockState>(Keys); } }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove((ILayoutBlockState)key); }

        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value)
        {
            bool Result = TryGetValue((ILayoutBlockState)key, out ILayoutBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { return new List<IWriteableBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { Add((ILayoutBlockState)item.Key, (ILayoutBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey((ILayoutBlockState)item.Key) && this[(ILayoutBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return Remove((ILayoutBlockState)item.Key); }

        IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> NewList = new List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Frame
        Dictionary<IFrameBlockState, IFrameBlockStateView>.Enumerator IFrameBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFrameBlockState, IFrameBlockStateView> NewDictionary = new Dictionary<IFrameBlockState, IFrameBlockStateView>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFrameBlockStateView IDictionary<IFrameBlockState, IFrameBlockStateView>.this[IFrameBlockState key] { get { return this[(ILayoutBlockState)key]; } set { this[(ILayoutBlockState)key] = (ILayoutBlockStateView)value; } }
        void IDictionary<IFrameBlockState, IFrameBlockStateView>.Add(IFrameBlockState key, IFrameBlockStateView value) { Add((ILayoutBlockState)key, (ILayoutBlockStateView)value); }
        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.ContainsKey(IFrameBlockState key) { return ContainsKey((ILayoutBlockState)key); }
        ICollection<IFrameBlockState> IDictionary<IFrameBlockState, IFrameBlockStateView>.Keys { get { return new List<IFrameBlockState>(Keys); } }
        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.Remove(IFrameBlockState key) { return Remove((ILayoutBlockState)key); }

        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.TryGetValue(IFrameBlockState key, out IFrameBlockStateView value)
        {
            bool Result = TryGetValue((ILayoutBlockState)key, out ILayoutBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IFrameBlockStateView> IDictionary<IFrameBlockState, IFrameBlockStateView>.Values { get { return new List<IFrameBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Add(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { Add((ILayoutBlockState)item.Key, (ILayoutBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Contains(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { return ContainsKey((ILayoutBlockState)item.Key) && this[(ILayoutBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.CopyTo(KeyValuePair<IFrameBlockState, IFrameBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFrameBlockState, IFrameBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Remove(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { return Remove((ILayoutBlockState)item.Key); }

        IEnumerator<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> IEnumerable<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> NewList = new List<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameBlockState, IFrameBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Focus
        Dictionary<IFocusBlockState, IFocusBlockStateView>.Enumerator IFocusBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFocusBlockState, IFocusBlockStateView> NewDictionary = new Dictionary<IFocusBlockState, IFocusBlockStateView>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        IFocusBlockStateView IDictionary<IFocusBlockState, IFocusBlockStateView>.this[IFocusBlockState key] { get { return this[(ILayoutBlockState)key]; } set { this[(ILayoutBlockState)key] = (ILayoutBlockStateView)value; } }
        void IDictionary<IFocusBlockState, IFocusBlockStateView>.Add(IFocusBlockState key, IFocusBlockStateView value) { Add((ILayoutBlockState)key, (ILayoutBlockStateView)value); }
        bool IDictionary<IFocusBlockState, IFocusBlockStateView>.ContainsKey(IFocusBlockState key) { return ContainsKey((ILayoutBlockState)key); }
        ICollection<IFocusBlockState> IDictionary<IFocusBlockState, IFocusBlockStateView>.Keys { get { return new List<IFocusBlockState>(Keys); } }
        bool IDictionary<IFocusBlockState, IFocusBlockStateView>.Remove(IFocusBlockState key) { return Remove((ILayoutBlockState)key); }

        bool IDictionary<IFocusBlockState, IFocusBlockStateView>.TryGetValue(IFocusBlockState key, out IFocusBlockStateView value)
        {
            bool Result = TryGetValue((ILayoutBlockState)key, out ILayoutBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IFocusBlockStateView> IDictionary<IFocusBlockState, IFocusBlockStateView>.Values { get { return new List<IFocusBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.Add(KeyValuePair<IFocusBlockState, IFocusBlockStateView> item) { Add((ILayoutBlockState)item.Key, (ILayoutBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.Contains(KeyValuePair<IFocusBlockState, IFocusBlockStateView> item) { return ContainsKey((ILayoutBlockState)item.Key) && this[(ILayoutBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.CopyTo(KeyValuePair<IFocusBlockState, IFocusBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IFocusBlockState, IFocusBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.Remove(KeyValuePair<IFocusBlockState, IFocusBlockStateView> item) { return Remove((ILayoutBlockState)item.Key); }

        IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> IEnumerable<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> NewList = new List<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>();
            IEnumerator<KeyValuePair<ILayoutBlockState, ILayoutBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFocusBlockState, IFocusBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<ILayoutBlockState, ILayoutBlockStateView> Entry in this)
            {
                ILayoutBlockState Key = Entry.Key;
                ILayoutBlockStateView Value = Entry.Value;

                if (!comparer.IsTrue(AsBlockStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
