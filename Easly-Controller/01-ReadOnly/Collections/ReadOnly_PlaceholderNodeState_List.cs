namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;

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
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyPlaceholderNodeStateList AsPlaceholderNodeStateList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsPlaceholderNodeStateList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
