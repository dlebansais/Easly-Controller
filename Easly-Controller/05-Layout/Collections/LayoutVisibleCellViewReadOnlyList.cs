namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutVisibleCellView> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameVisibleCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutVisibleCellView)iterator.Current; } }

        #region ILayoutVisibleCellView
        IEnumerator<ILayoutVisibleCellView> IEnumerable<ILayoutVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        ILayoutVisibleCellView IReadOnlyList<ILayoutVisibleCellView>.this[int index] { get { return (ILayoutVisibleCellView)this[index]; } }
        #endregion
    }
}
