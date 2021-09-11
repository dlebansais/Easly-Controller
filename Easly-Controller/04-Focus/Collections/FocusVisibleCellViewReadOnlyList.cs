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

        /// <inheritdoc/>
        public new IFocusVisibleCellView this[int index] { get { return (IFocusVisibleCellView)base[index]; } }

        #region IFocusVisibleCellView
        IEnumerator<IFocusVisibleCellView> IEnumerable<IFocusVisibleCellView>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusVisibleCellView)iterator.Current; } }
        IFocusVisibleCellView IReadOnlyList<IFocusVisibleCellView>.this[int index] { get { return (IFocusVisibleCellView)this[index]; } }
        #endregion
    }
}
