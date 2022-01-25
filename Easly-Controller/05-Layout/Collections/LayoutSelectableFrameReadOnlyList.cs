namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutSelectableFrame> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFocusSelectableFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutSelectableFrame)iterator.Current; } }

        #region ILayoutSelectableFrame
        IEnumerator<ILayoutSelectableFrame> IEnumerable<ILayoutSelectableFrame>.GetEnumerator() { return GetEnumerator(); }
        ILayoutSelectableFrame IReadOnlyList<ILayoutSelectableFrame>.this[int index] { get { return (ILayoutSelectableFrame)this[index]; } }
        #endregion
    }
}
