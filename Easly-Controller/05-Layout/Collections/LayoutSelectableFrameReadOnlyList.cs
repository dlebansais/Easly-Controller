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
        IEnumerator<ILayoutSelectableFrame> IEnumerable<ILayoutSelectableFrame>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutSelectableFrame)iterator.Current; } }
        ILayoutSelectableFrame IReadOnlyList<ILayoutSelectableFrame>.this[int index] { get { return (ILayoutSelectableFrame)this[index]; } }
        #endregion
    }
}
