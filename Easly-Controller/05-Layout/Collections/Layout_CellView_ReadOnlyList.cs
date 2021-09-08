namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutCellViewReadOnlyList : FocusCellViewReadOnlyList, IReadOnlyCollection<ILayoutCellView>, IReadOnlyList<ILayoutCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutCellViewReadOnlyList(LayoutCellViewList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutCellView this[int index] { get { return (ILayoutCellView)base[index]; } }

        #region ILayoutCellView
        IEnumerator<ILayoutCellView> IEnumerable<ILayoutCellView>.GetEnumerator() { return ((IList<ILayoutCellView>)this).GetEnumerator(); }
        ILayoutCellView IReadOnlyList<ILayoutCellView>.this[int index] { get { return (ILayoutCellView)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutCellViewReadOnlyList AsCellViewReadOnlyList))
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
