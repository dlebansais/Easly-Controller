namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBlockStateViewReadOnlyDictionary : FrameBlockStateViewReadOnlyDictionary, ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IEnumerable<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IDictionary<IFocusBlockState, FocusBlockStateView>, IReadOnlyCollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>, IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusBlockStateViewReadOnlyDictionary(FocusBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        /// <inheritdoc/>
        public bool TryGetValue(IFocusBlockState key, out FocusBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FocusBlockStateView)Value; return Result; }

        #region IFocusBlockState, FocusBlockStateView
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Add(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { throw new System.InvalidOperationException(); }
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Clear() { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Contains(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { return ContainsKey(item.Key) && this[item.Key] == item.Value; }
        void ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.CopyTo(KeyValuePair<IFocusBlockState, FocusBlockStateView>[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.Remove(KeyValuePair<IFocusBlockState, FocusBlockStateView> item) { throw new System.InvalidOperationException(); }
        bool ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.IsReadOnly { get { return false; } }
        IEnumerator<KeyValuePair<IFocusBlockState, FocusBlockStateView>> IEnumerable<KeyValuePair<IFocusBlockState, FocusBlockStateView>>.GetEnumerator() { return ((IList<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this).GetEnumerator(); }

        FocusBlockStateView IDictionary<IFocusBlockState, FocusBlockStateView>.this[IFocusBlockState key] { get { return (FocusBlockStateView)this[key]; } set { throw new System.InvalidOperationException(); } }
        ICollection<IFocusBlockState> IDictionary<IFocusBlockState, FocusBlockStateView>.Keys { get { List<IFocusBlockState> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        ICollection<FocusBlockStateView> IDictionary<IFocusBlockState, FocusBlockStateView>.Values { get { List<FocusBlockStateView> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        void IDictionary<IFocusBlockState, FocusBlockStateView>.Add(IFocusBlockState key, FocusBlockStateView value) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.ContainsKey(IFocusBlockState key) { return ContainsKey(key); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.Remove(IFocusBlockState key) { throw new System.InvalidOperationException(); }
        bool IDictionary<IFocusBlockState, FocusBlockStateView>.TryGetValue(IFocusBlockState key, out FocusBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FocusBlockStateView)Value; return Result; }

        FocusBlockStateView IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.this[IFocusBlockState key] { get { return (FocusBlockStateView)this[key]; } }
        IEnumerable<IFocusBlockState> IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.Keys { get { List<IFocusBlockState> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Key); return Result; } }
        IEnumerable<FocusBlockStateView> IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.Values { get { List<FocusBlockStateView> Result = new(); foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)this) Result.Add(Entry.Value); return Result; } }
        bool IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.ContainsKey(IFocusBlockState key) { return ContainsKey(key); }
        bool IReadOnlyDictionary<IFocusBlockState, FocusBlockStateView>.TryGetValue(IFocusBlockState key, out FocusBlockStateView value) { bool Result = TryGetValue(key, out ReadOnlyBlockStateView Value); value = (FocusBlockStateView)Value; return Result; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusBlockStateViewReadOnlyDictionary AsBlockStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (IFocusBlockState Key in Keys)
            {
                FocusBlockStateView Value = (FocusBlockStateView)this[Key];

                if (!comparer.IsTrue(AsBlockStateViewReadOnlyDictionary.ContainsKey(Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Value, AsBlockStateViewReadOnlyDictionary[Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
