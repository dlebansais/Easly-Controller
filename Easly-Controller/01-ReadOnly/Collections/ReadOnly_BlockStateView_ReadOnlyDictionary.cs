namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>, IEqualComparable
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
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateViewReadOnlyDictionary AsBlockStateViewReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsBlockStateViewReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsBlockStateViewReadOnlyDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
