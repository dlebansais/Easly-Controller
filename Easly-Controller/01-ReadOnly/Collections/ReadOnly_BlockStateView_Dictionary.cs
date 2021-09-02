namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyBlockStateViewDictionary : Dictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual ReadOnlyBlockStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyBlockStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBlockStateViewDictionary"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateViewDictionary AsBlockStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsBlockStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsBlockStateViewDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsBlockStateViewDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
