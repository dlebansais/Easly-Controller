namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutSelectableFrameReadOnlyList : FocusSelectableFrameReadOnlyList, IReadOnlyCollection<ILayoutSelectableFrame>, IReadOnlyList<ILayoutSelectableFrame>
    {
        /// <inheritdoc/>
        public LayoutSelectableFrameReadOnlyList(LayoutSelectableFrameList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutSelectableFrame this[int index] { get { return (ILayoutSelectableFrame)base[index]; } }

        #region ILayoutSelectableFrame
        IEnumerator<ILayoutSelectableFrame> IEnumerable<ILayoutSelectableFrame>.GetEnumerator() { return ((IList<ILayoutSelectableFrame>)this).GetEnumerator(); }
        ILayoutSelectableFrame IReadOnlyList<ILayoutSelectableFrame>.this[int index] { get { return (ILayoutSelectableFrame)this[index]; } }
        #endregion
    }
}
