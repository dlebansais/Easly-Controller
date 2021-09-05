namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusVisibleCellViewReadOnlyList : FrameVisibleCellViewReadOnlyList, IReadOnlyCollection<IFocusVisibleCellView>, IReadOnlyList<IFocusVisibleCellView>
    {
        /// <inheritdoc/>
        public FocusVisibleCellViewReadOnlyList(FocusVisibleCellViewList list)
            : base(list)
        {
        }

        #region IFocusVisibleCellView
        IEnumerator<IFocusVisibleCellView> IEnumerable<IFocusVisibleCellView>.GetEnumerator() { return ((IList<IFocusVisibleCellView>)this).GetEnumerator(); }
        IFocusVisibleCellView IReadOnlyList<IFocusVisibleCellView>.this[int index] { get { return (IFocusVisibleCellView)this[index]; } }
        #endregion
    }
}
