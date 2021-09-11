namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

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
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameCellViewReadOnlyList AsOtherReadOnlyList))
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
