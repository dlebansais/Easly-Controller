namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewDictionary : Dictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(IDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(IEqualityComparer<IReadOnlyBlockState> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(IDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView> dictionary, IEqualityComparer<IReadOnlyBlockState> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(int capacity, IEqualityComparer<IReadOnlyBlockState> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyBlockStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateViewDictionary AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsOtherDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
