namespace EaslyController.ReadOnly
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyPlaceholderNodeStateReadOnlyList(ReadOnlyPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyPlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsPlaceholderNodeStateReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
