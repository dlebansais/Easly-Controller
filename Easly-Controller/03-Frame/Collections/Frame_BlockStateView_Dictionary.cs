using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public interface IFrameBlockStateViewDictionary : IWriteableBlockStateViewDictionary, IDictionary<IFrameBlockState, IFrameBlockStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IFrameBlockState, IFrameBlockStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public class FrameBlockStateViewDictionary : Dictionary<IFrameBlockState, IFrameBlockStateView>, IFrameBlockStateViewDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((IFrameBlockState)key, (IFrameBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((IFrameBlockState)key); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value) { bool Result = TryGetValue((IFrameBlockState)key, out IFrameBlockStateView Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((IFrameBlockState)key); }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((IFrameBlockState)item.Key, (IFrameBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((IFrameBlockState)item.Key) && base[(IFrameBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((IFrameBlockState)item.Key); }
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(IFrameBlockState)key]; } set { this[(IFrameBlockState)key] = (IFrameBlockStateView)value; } }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        public void Add(IFrameBlockState key, IReadOnlyBlockStateView value) { base.Add(key, (IFrameBlockStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> IEnumerable<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> NewList = new List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>();
            IEnumerator<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IFrameBlockState key, out IReadOnlyBlockStateView value) { bool Result = TryGetValue(key, out IFrameBlockStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFrameBlockState, IReadOnlyBlockStateView> item) { base.Add(item.Key, (IFrameBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IFrameBlockState, IReadOnlyBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFrameBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFrameBlockState, IReadOnlyBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Writeable
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { Add((IFrameBlockState)key, (IFrameBlockStateView)value); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove((IFrameBlockState)key); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue((IFrameBlockState)key, out IFrameBlockStateView Value); value = Value; return Result; }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey((IFrameBlockState)key); }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { Add((IFrameBlockState)item.Key, (IFrameBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey((IFrameBlockState)item.Key) && base[(IFrameBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return Remove((IFrameBlockState)item.Key); }
        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return this[(IFrameBlockState)key]; } set { this[(IFrameBlockState)key] = (IFrameBlockStateView)value; } }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { return new List<IWriteableBlockState>(Keys); } }
        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { return new List<IWriteableBlockStateView>(Values); } }
        public void Add(IFrameBlockState key, IWriteableBlockStateView value) { base.Add(key, (IFrameBlockStateView)value); }

        IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> IEnumerable<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> NewList = new List<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>();
            IEnumerator<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        Dictionary<IWriteableBlockState, IWriteableBlockStateView>.Enumerator IWriteableBlockStateViewDictionary.GetEnumerator()
        {
            Dictionary<IWriteableBlockState, IWriteableBlockStateView> NewDictionary = new Dictionary<IWriteableBlockState, IWriteableBlockStateView>();
            IEnumerator<KeyValuePair<IFrameBlockState, IFrameBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry = Enumerator.Current;
                NewDictionary.Add(Entry.Key, Entry.Value);
            }

            return NewDictionary.GetEnumerator();
        }

        public bool TryGetValue(IFrameBlockState key, out IWriteableBlockStateView value) { bool Result = TryGetValue(key, out IFrameBlockStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IFrameBlockState, IWriteableBlockStateView> item) { base.Add(item.Key, (IFrameBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IFrameBlockState, IWriteableBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IFrameBlockState, IWriteableBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IFrameBlockState, IWriteableBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBlockStateViewDictionary AsBlockStateViewDictionary))
                return false;

            if (Count != AsBlockStateViewDictionary.Count)
                return false;

            foreach (KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry in this)
            {
                IFrameBlockState Key = Entry.Key;
                IFrameBlockStateView Value = Entry.Value;

                if (!AsBlockStateViewDictionary.ContainsKey(Key))
                    return false;

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
