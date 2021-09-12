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
        /// <inheritdoc/>
        public new IEnumerator<IFocusVisibleCellView> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFrameVisibleCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusVisibleCellView)iterator.Current; } }

        #region IFocusVisibleCellView
        IEnumerator<IFocusVisibleCellView> IEnumerable<IFocusVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        IFocusVisibleCellView IReadOnlyList<IFocusVisibleCellView>.this[int index] { get { return (IFocusVisibleCellView)this[index]; } }
        #endregion
    }
}
