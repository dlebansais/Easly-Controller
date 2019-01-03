using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public interface IWriteableBlockStateViewDictionary : IReadOnlyBlockStateViewDictionary, IDictionary<IWriteableBlockState, IWriteableBlockStateView>, IEqualComparable
    {
        new int Count { get; }
        new Dictionary<IWriteableBlockState, IWriteableBlockStateView>.Enumerator GetEnumerator();
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxBlockState
    /// </summary>
    public class WriteableBlockStateViewDictionary : Dictionary<IWriteableBlockState, IWriteableBlockStateView>, IWriteableBlockStateViewDictionary
    {
        #region ReadOnly
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((IWriteableBlockState)key, (IWriteableBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((IWriteableBlockState)key); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value) { bool Result = TryGetValue((IWriteableBlockState)key, out IWriteableBlockStateView Value); value = Value; return Result; }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((IWriteableBlockState)key); }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((IWriteableBlockState)item.Key, (IWriteableBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((IWriteableBlockState)item.Key) && base[(IWriteableBlockState)item.Key] == item.Value; }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((IWriteableBlockState)item.Key); }
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(IWriteableBlockState)key]; } set { this[(IWriteableBlockState)key] = (IWriteableBlockStateView)value; } }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        public void Add(IWriteableBlockState key, IReadOnlyBlockStateView value) { base.Add(key, (IWriteableBlockStateView)value); }

        IEnumerator<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> IEnumerable<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.GetEnumerator()
        {
            List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>> NewList = new List<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>();
            IEnumerator<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>> Enumerator = GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry = Enumerator.Current;
                NewList.Add(new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value));
            }

            return NewList.GetEnumerator();
        }

        public bool TryGetValue(IWriteableBlockState key, out IReadOnlyBlockStateView value) { bool Result = TryGetValue(key, out IWriteableBlockStateView Value); value = Value; return Result; }
        public void Add(KeyValuePair<IWriteableBlockState, IReadOnlyBlockStateView> item) { base.Add(item.Key, (IWriteableBlockStateView)item.Value); }
        public bool Contains(KeyValuePair<IWriteableBlockState, IReadOnlyBlockStateView> item) { return ContainsKey(item.Key) && base[item.Key] == item.Value; }
        public void CopyTo(KeyValuePair<IWriteableBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex) { throw new InvalidOperationException(); }
        public bool Remove(KeyValuePair<IWriteableBlockState, IReadOnlyBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this).IsReadOnly; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableBlockStateViewDictionary AsBlockStateViewDictionary))
                return false;

            if (Count != AsBlockStateViewDictionary.Count)
                return false;

            foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in this)
            {
                IWriteableBlockState Key = Entry.Key;
                IWriteableBlockStateView Value = Entry.Value;

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
