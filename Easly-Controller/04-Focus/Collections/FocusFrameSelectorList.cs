namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusFrameSelectorList : List<IFocusFrameSelector>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual FocusFrameSelectorReadOnlyList ToReadOnly()
        {
            return new FocusFrameSelectorReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusFrameSelectorList AsOtherList))
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
