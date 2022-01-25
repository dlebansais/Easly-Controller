namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using Contracts;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewDictionary : Dictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(IDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyBlockStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBlockStateViewDictionary AsOtherDictionary))
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
