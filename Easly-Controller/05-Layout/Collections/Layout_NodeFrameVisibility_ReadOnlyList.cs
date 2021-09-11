﻿namespace EaslyController.Layout
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

        /// <inheritdoc/>
        public new ILayoutNodeFrameVisibility this[int index] { get { return (ILayoutNodeFrameVisibility)base[index]; } }

        #region ILayoutNodeFrameVisibility
        IEnumerator<ILayoutNodeFrameVisibility> IEnumerable<ILayoutNodeFrameVisibility>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutNodeFrameVisibility)iterator.Current; } }
        ILayoutNodeFrameVisibility IReadOnlyList<ILayoutNodeFrameVisibility>.this[int index] { get { return (ILayoutNodeFrameVisibility)this[index]; } }
        #endregion
    }
}