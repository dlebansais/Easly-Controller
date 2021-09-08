namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameBlockStateViewDictionary : WriteableBlockStateViewDictionary, ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IEnumerable<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IDictionary<IFrameBlockState, FrameBlockStateView>, IReadOnlyCollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>, IReadOnlyDictionary<IFrameBlockState, FrameBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public bool TryGetValue(IFrameBlockState key, out FrameBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FrameBlockStateView)Value; return Result; }

        #region IFrameBlockState, FrameBlockStateView
        void ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Add(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { Add(item.Key, item.Value); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Contains(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.CopyTo(KeyValuePair<IFrameBlockState, FrameBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.Remove(KeyValuePair<IFrameBlockState, FrameBlockStateView> item) { return Remove(item.Key); }
        bool ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.IsReadOnly { get { return ((ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)this).IsReadOnly; } }
        IEnumerator<KeyValuePair<IFrameBlockState, FrameBlockStateView>> IEnumerable<KeyValuePair<IFrameBlockState, FrameBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)this).GetEnumerator(); }

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

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (IFrameBlockState Key in Keys)
            {
                FrameBlockStateView Value = (FrameBlockStateView)this[Key];

                if (!comparer.IsTrue(AsBlockStateViewDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new FrameBlockStateViewReadOnlyDictionary(this);
        }
    }
}
