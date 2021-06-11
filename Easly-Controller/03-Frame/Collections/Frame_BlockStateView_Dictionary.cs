#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

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
    internal class FrameBlockStateViewDictionary : Dictionary<IFrameBlockState, IFrameBlockStateView>, IFrameBlockStateViewDictionary
    {
        #region ReadOnly
        IReadOnlyBlockStateView IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.this[IReadOnlyBlockState key] { get { return this[(IFrameBlockState)key]; } set { this[(IFrameBlockState)key] = (IFrameBlockStateView)value; } }
        void IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Add(IReadOnlyBlockState key, IReadOnlyBlockStateView value) { Add((IFrameBlockState)key, (IFrameBlockStateView)value); }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.ContainsKey(IReadOnlyBlockState key) { return ContainsKey((IFrameBlockState)key); }
        ICollection<IReadOnlyBlockState> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Keys { get { return new List<IReadOnlyBlockState>(Keys); } }
        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Remove(IReadOnlyBlockState key) { return Remove((IFrameBlockState)key); }

        bool IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.TryGetValue(IReadOnlyBlockState key, out IReadOnlyBlockStateView value)
        {
            bool Result = TryGetValue((IFrameBlockState)key, out IFrameBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IReadOnlyBlockStateView> IDictionary<IReadOnlyBlockState, IReadOnlyBlockStateView>.Values { get { return new List<IReadOnlyBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Add(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { Add((IFrameBlockState)item.Key, (IFrameBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Contains(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return ContainsKey((IFrameBlockState)item.Key) && this[(IFrameBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.CopyTo(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView>>.Remove(KeyValuePair<IReadOnlyBlockState, IReadOnlyBlockStateView> item) { return Remove((IFrameBlockState)item.Key); }

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
        #endregion

        #region Writeable
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

        IWriteableBlockStateView IDictionary<IWriteableBlockState, IWriteableBlockStateView>.this[IWriteableBlockState key] { get { return this[(IFrameBlockState)key]; } set { this[(IFrameBlockState)key] = (IFrameBlockStateView)value; } }
        void IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Add(IWriteableBlockState key, IWriteableBlockStateView value) { Add((IFrameBlockState)key, (IFrameBlockStateView)value); }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.ContainsKey(IWriteableBlockState key) { return ContainsKey((IFrameBlockState)key); }
        ICollection<IWriteableBlockState> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Keys { get { return new List<IWriteableBlockState>(Keys); } }
        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Remove(IWriteableBlockState key) { return Remove((IFrameBlockState)key); }

        bool IDictionary<IWriteableBlockState, IWriteableBlockStateView>.TryGetValue(IWriteableBlockState key, out IWriteableBlockStateView value)
        {
            bool Result = TryGetValue((IFrameBlockState)key, out IFrameBlockStateView Value);
            value = Value;
            return Result;
        }

        ICollection<IWriteableBlockStateView> IDictionary<IWriteableBlockState, IWriteableBlockStateView>.Values { get { return new List<IWriteableBlockStateView>(Values); } }
        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Add(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { Add((IFrameBlockState)item.Key, (IFrameBlockStateView)item.Value); }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Contains(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return ContainsKey((IFrameBlockState)item.Key) && this[(IFrameBlockState)item.Key] == item.Value; }

        void ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.CopyTo(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry in this)
                array[arrayIndex++] = new KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>(Entry.Key, Entry.Value);
        }

        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IFrameBlockState, IFrameBlockStateView>>)this).IsReadOnly; } }
        bool ICollection<KeyValuePair<IWriteableBlockState, IWriteableBlockStateView>>.Remove(KeyValuePair<IWriteableBlockState, IWriteableBlockStateView> item) { return Remove((IFrameBlockState)item.Key); }

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
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry in this)
            {
                IFrameBlockState Key = Entry.Key;
                IFrameBlockStateView Value = Entry.Value;

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
