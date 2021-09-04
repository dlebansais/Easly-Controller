namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class FrameCellViewReadOnlyList : ReadOnlyCollection<IFrameCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FrameCellViewReadOnlyList(FrameCellViewList list)
            : base(list)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameCellViewReadOnlyList AsCellViewReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsCellViewReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
