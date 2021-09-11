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

        /// <inheritdoc/>
        public new ILayoutVisibleCellView this[int index] { get { return (ILayoutVisibleCellView)base[index]; } }

        #region ILayoutVisibleCellView
        IEnumerator<ILayoutVisibleCellView> IEnumerable<ILayoutVisibleCellView>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutVisibleCellView)iterator.Current; } }
        ILayoutVisibleCellView IReadOnlyList<ILayoutVisibleCellView>.this[int index] { get { return (ILayoutVisibleCellView)this[index]; } }
        #endregion
    }
}
