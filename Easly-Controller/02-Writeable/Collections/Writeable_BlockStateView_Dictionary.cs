#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

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
    internal class WriteableBlockStateViewDictionary : Dictionary<IWriteableBlockState, IWriteableBlockStateView>, IWriteableBlockStateViewDictionary
    {
        #region ReadOnly
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(IWriteableBlockState)key]; } set { this[(IWriteableBlockState)key] = (IWriteableBlockStateView)value; } }
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((IWriteableBlockState)key, (IWriteableBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((IWriteableBlockState)key); }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((IWriteableBlockState)key); }

        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value)
        {
            bool Result = TryGetValue((IWriteableBlockState)key, out IWriteableBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((IWriteableBlockState)item.Key, (IWriteableBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((IWriteableBlockState)item.Key) && this[(IWriteableBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((IWriteableBlockState)item.Key); }

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
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> Entry in this)
            {
                IWriteableBlockState Key = Entry.Key;
                IWriteableBlockStateView Value = Entry.Value;

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
