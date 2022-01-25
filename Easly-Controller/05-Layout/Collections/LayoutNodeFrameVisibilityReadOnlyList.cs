namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutNodeFrameVisibilityReadOnlyList : FocusNodeFrameVisibilityReadOnlyList, IReadOnlyCollection<ILayoutNodeFrameVisibility>, IReadOnlyList<ILayoutNodeFrameVisibility>
    {
        /// <inheritdoc/>
        public LayoutNodeFrameVisibilityReadOnlyList(LayoutNodeFrameVisibilityList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutNodeFrameVisibility this[int index] { get { return (ILayoutNodeFrameVisibility)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutNodeFrameVisibility> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFocusNodeFrameVisibility>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutNodeFrameVisibility)iterator.Current; } }

        #region ILayoutNodeFrameVisibility
        IEnumerator<ILayoutNodeFrameVisibility> IEnumerable<ILayoutNodeFrameVisibility>.GetEnumerator() { return GetEnumerator(); }
        ILayoutNodeFrameVisibility IReadOnlyList<ILayoutNodeFrameVisibility>.this[int index] { get { return (ILayoutNodeFrameVisibility)this[index]; } }
        #endregion
    }
}
