namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutCellView> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFrameCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutCellView)iterator.Current; } }

        #region ILayoutCellView
        IEnumerator<ILayoutCellView> IEnumerable<ILayoutCellView>.GetEnumerator() { return GetEnumerator(); }
        ILayoutCellView IReadOnlyList<ILayoutCellView>.this[int index] { get { return (ILayoutCellView)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutCellViewReadOnlyList AsOtherReadOnlyList))
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
