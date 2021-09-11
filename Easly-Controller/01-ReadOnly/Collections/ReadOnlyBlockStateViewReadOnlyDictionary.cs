namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewReadOnlyDictionary : System.Collections.ObjectModel.ReadOnlyDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyBlockStateViewReadOnlyDictionary(ReadOnlyBlockStateViewDictionary dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateViewReadOnlyDictionary AsOtherReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsOtherReadOnlyDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
