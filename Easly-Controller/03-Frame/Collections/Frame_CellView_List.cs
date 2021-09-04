namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class FrameCellViewList : List<IFrameCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual FrameCellViewReadOnlyList ToReadOnly()
        {
            return new FrameCellViewReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameCellViewList AsCellViewList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsCellViewList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
