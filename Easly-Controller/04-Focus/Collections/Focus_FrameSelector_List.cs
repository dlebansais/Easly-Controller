﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class FocusFrameSelectorList : List<IFocusFrameSelector>, IEqualComparable
    {
        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusFrameSelectorList AsFrameSelectorList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsFrameSelectorList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsFrameSelectorList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public virtual FocusFrameSelectorReadOnlyList ToReadOnly()
        {
            return new FocusFrameSelectorReadOnlyList(this);
        }
    }
}
