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
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public interface IFocusBlockStateViewDictionary : IFrameBlockStateViewDictionary, IDictionary<IFocusBlockState, IFocusBlockStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IFocusBlockState, IFocusBlockStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public class FocusBlockStateViewDictionary : Dictionary<IFocusBlockState, IFocusBlockStateView>, IFocusBlockStateViewDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((IFocusBlockState)key, (IFocusBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((IFocusBlockState)key); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value)
        {
            bool Result = TryGetValue((IFocusBlockState)key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((IFocusBlockState)key); }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((IFocusBlockState)item.Key, (IFocusBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((IFocusBlockState)item.Key) && base[(IFocusBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((IFocusBlockState)item.Key); }
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(IFocusBlockState)key]; } set { this[(IFocusBlockState)key] = (IFocusBlockStateView)value; } }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        public void Add(IFocusBlockState key, IReadOnlyBlockStateView value) { base.Add(key, (IFocusBlockStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> IEnumerable<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> NewList = new List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>();
            IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IFocusBlockState key, out IReadOnlyBlockStateView value)
        {
            bool Result = TryGetValue(key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<IFocusBlockState, IReadOnlyBlockStateView> item) { base.Add(item.Key, (IFocusBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusBlockState, IReadOnlyBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusBlockState, IReadOnlyBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { Add((IFocusBlockState)key, (IFocusBlockStateView)value); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove((IFocusBlockState)key); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value)
        {
            bool Result = TryGetValue((IFocusBlockState)key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey((IFocusBlockState)key); }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { Add((IFocusBlockState)item.Key, (IFocusBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey((IFocusBlockState)item.Key) && base[(IFocusBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return Remove((IFocusBlockState)item.Key); }
        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return this[(IFocusBlockState)key]; } set { this[(IFocusBlockState)key] = (IFocusBlockStateView)value; } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { return new List<IWriteableBlockState>(Keys); } }
        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { return new List<IWriteableBlockStateView>(Values); } }
        public void Add(IFocusBlockState key, IWriteableBlockStateView value) { base.Add(key, (IFocusBlockStateView)value); }

        IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> NewList = new List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>();
            IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableBlockState, IWriteableBlockStateView>.Enumerator IWriteableBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableBlockState, IWriteableBlockStateView> NewDictionary = new Dictionary<IWriteableBlockState, IWriteableBlockStateView>();
            IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusBlockState key, out IWriteableBlockStateView value)
        {
            bool Result = TryGetValue(key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<IFocusBlockState, IWriteableBlockStateView> item) { base.Add(item.Key, (IFocusBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusBlockState, IWriteableBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusBlockState, IWriteableBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Frame
        void IDictionary<IFrameBlockState, IFrameBlockStateView>.Add(IFrameBlockState key, IFrameBlockStateView value) { Add((IFocusBlockState)key, (IFocusBlockStateView)value); }
        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.Remove(IFrameBlockState key) { return Remove((IFocusBlockState)key); }
        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.TryGetValue(IFrameBlockState key, out IFrameBlockStateView value)
        {
            bool Result = TryGetValue((IFocusBlockState)key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        bool IDictionary<IFrameBlockState, IFrameBlockStateView>.ContainsKey(IFrameBlockState key) { return ContainsKey((IFocusBlockState)key); }
        void ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Add(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { Add((IFocusBlockState)item.Key, (IFocusBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Contains(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { return ContainsKey((IFocusBlockState)item.Key) && base[(IFocusBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.CopyTo(KeyValuePair<IFrameBlockState, IFrameBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.Remove(KeyValuePair<IFrameBlockState, IFrameBlockStateView> item) { return Remove((IFocusBlockState)item.Key); }
        IFrameBlockStateView IDictionary<IFrameBlockState, IFrameBlockStateView>.this[IFrameBlockState key] { get { return this[(IFocusBlockState)key]; } set { this[(IFocusBlockState)key] = (IFocusBlockStateView)value; } }
        ICollection<IFrameBlockState> IDictionary<IFrameBlockState, IFrameBlockStateView>.Keys { get { return new List<IFrameBlockState>(Keys); } }
        ICollection<IFrameBlockStateView> IDictionary<IFrameBlockState, IFrameBlockStateView>.Values { get { return new List<IFrameBlockStateView>(Values); } }
        public void Add(IFocusBlockState key, IFrameBlockStateView value) { base.Add(key, (IFocusBlockStateView)value); }

        IEnumerator<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> IEnumerable<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> NewList = new List<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>();
            IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IFrameBlockState, IFrameBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IFrameBlockState, IFrameBlockStateView>.Enumerator IFrameBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IFrameBlockState, IFrameBlockStateView> NewDictionary = new Dictionary<IFrameBlockState, IFrameBlockStateView>();
            IEnumerator<KeyValuePair<IFocusBlockState, IFocusBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFocusBlockState key, out IFrameBlockStateView value)
        {
            bool Result = TryGetValue(key, out IFocusBlockStateView Value);
            value = Value;
            return Result;
        }
        public void Add(KeyValuePair<IFocusBlockState, IFrameBlockStateView> item) { base.Add(item.Key, (IFocusBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IFocusBlockState, IFrameBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFocusBlockState, IFrameBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFocusBlockState, IFrameBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFocusBlockState, IFocusBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (Count != AsBlockStateViewDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<IFocusBlockState, IFocusBlockStateView> Entry in this)
            {
                IFocusBlockState Key = Entry.Key;
                IFocusBlockStateView Value = Entry.Value;

                if (!AsBlockStateViewDictionary.ContainsKey(Key))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
