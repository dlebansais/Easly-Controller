namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutVisibleCellViewReadOnlyList : FocusVisibleCellViewReadOnlyList, IReadOnlyCollection<ILayoutVisibleCellView>, IReadOnlyList<ILayoutVisibleCellView>
    {
        /// <inheritdoc/>
        public LayoutVisibleCellViewReadOnlyList(LayoutVisibleCellViewList list)
            : base(list)
        {
        }

        #region ILayoutVisibleCellView
        IEnumerator<ILayoutVisibleCellView> IEnumerable<ILayoutVisibleCellView>.GetEnumerator() { return ((IList<ILayoutVisibleCellView>)this).GetEnumerator(); }
        ILayoutVisibleCellView IReadOnlyList<ILayoutVisibleCellView>.this[int index] { get { return (ILayoutVisibleCellView)this[index]; } }
        #endregion
    }
}
