namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutNodeFrameVisibilityReadOnlyList : FocusNodeFrameVisibilityReadOnlyList, IReadOnlyCollection<ILayoutNodeFrameVisibility>, IReadOnlyList<ILayoutNodeFrameVisibility>
    {
        /// <inheritdoc/>
        public LayoutNodeFrameVisibilityReadOnlyList(LayoutNodeFrameVisibilityList list)
            : base(list)
        {
        }

        #region ILayoutNodeFrameVisibility
        IEnumerator<ILayoutNodeFrameVisibility> IEnumerable<ILayoutNodeFrameVisibility>.GetEnumerator() { return ((IList<ILayoutNodeFrameVisibility>)this).GetEnumerator(); }
        ILayoutNodeFrameVisibility IReadOnlyList<ILayoutNodeFrameVisibility>.this[int index] { get { return (ILayoutNodeFrameVisibility)this[index]; } }
        #endregion
    }
}
