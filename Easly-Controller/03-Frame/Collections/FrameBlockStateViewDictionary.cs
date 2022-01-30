namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using Contracts;

    /// <inheritdoc/>
    public class FrameBlockStateViewDictionary : WriteableBlockStateViewDictionary, ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IEnumerable<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IDictionary<IFrameBlockState, FrameBlockStateView>, IReadOnlyCollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameBlockStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public FrameBlockStateViewDictionary(IDictionary<IFrameBlockState, FrameBlockStateView> dictionary) : base() { foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in dictionary) Add(Entry.Key, Entry.Value); }
        /// <inheritdoc/>
        public FrameBlockStateViewDictionary(int capacity) : base(capacity) { }

        #region IFrameBlockState, FrameBlockStateView
        void ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Add(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Contains(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.CopyTo(KeyValuePair<IFrameBlockState, FrameBlockStateView>[] array, int arrayIndex) { int i = arrayIndex; foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this) array[i++] = new KeyValuePair<IFrameBlockState, FrameBlockStateView>((IFrameBlockState)Entry.Key, (FrameBlockStateView)Entry.Value); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Remove(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this).IsReadOnly; } }

        IEnumerator<KeyValuePair<IFrameBlockState, FrameBlockStateView>> IEnumerable<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.GetEnumerator() { IEnumerator<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return new KeyValuePair<IFrameBlockState, FrameBlockStateView>((IFrameBlockState)iterator.Current.Key, (FrameBlockStateView)iterator.Current.Value); } }

        FrameBlockStateView IDictionary<IFrameBlockState, FrameBlockStateView>.this[IFrameBlockState key] { get { return (FrameBlockStateView)this[key]; } set { this[key] = value; } }
        ICollection<IFrameBlockState> IDictionary<IFrameBlockState, FrameBlockStateView>.Keys { get { List<IFrameBlockState> Result = new(); foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in (ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FrameBlockStateView> IDictionary<IFrameBlockState, FrameBlockStateView>.Values { get { List<FrameBlockStateView> Result = new(); foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in (ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFrameBlockState, FrameBlockStateView>.Add(IFrameBlockState key, FrameBlockStateView value) { Add(key, value); }
        bool IDictionary<IFrameBlockState, FrameBlockStateView>.ContainsKey(IFrameBlockState key) { return ContainsKey(key); }
        bool IDictionary<IFrameBlockState, FrameBlockStateView>.Remove(IFrameBlockState key) { return Remove(key); }
        bool IDictionary<IFrameBlockState, FrameBlockStateView>.TryGetValue(IFrameBlockState key, out FrameBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FrameBlockStateView)Value; return Result; }

        FrameBlockStateView IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>.this[IFrameBlockState key] { get { return (FrameBlockStateView)this[key]; } }
        IEnumerable<IFrameBlockState> IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>.Keys { get { List<IFrameBlockState> Result = new(); foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in (ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<FrameBlockStateView> IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>.Values { get { List<FrameBlockStateView> Result = new(); foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in (ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>.ContainsKey(IFrameBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>.TryGetValue(IFrameBlockState key, out FrameBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FrameBlockStateView)Value; return Result; }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new FrameBlockStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameBlockStateViewDictionary AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (IFrameBlockState Key in Keys)
            {
                FrameBlockStateView Value = (FrameBlockStateView)this[Key];

                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsOtherDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
