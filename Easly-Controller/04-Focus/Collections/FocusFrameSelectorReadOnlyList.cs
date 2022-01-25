namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;
    using Contracts;

    /// <inheritdoc/>
    public class FocusFrameSelectorReadOnlyList : ReadOnlyCollection<IFocusFrameSelector>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusFrameSelectorReadOnlyList(FocusFrameSelectorList list)
            : base(list)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusFrameSelectorReadOnlyList AsOtherReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsOtherReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
