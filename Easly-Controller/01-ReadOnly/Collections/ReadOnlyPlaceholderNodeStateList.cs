namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using Contracts;

    /// <inheritdoc/>
    public class ReadOnlyPlaceholderNodeStateList : List<IReadOnlyPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyPlaceholderNodeStateList AsOtherList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsOtherList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
