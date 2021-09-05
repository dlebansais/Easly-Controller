namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

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
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusFrameSelectorReadOnlyList AsFrameSelectorReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsFrameSelectorReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsFrameSelectorReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
