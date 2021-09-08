﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutFrameReadOnlyList : FocusFrameReadOnlyList, IReadOnlyCollection<ILayoutFrame>, IReadOnlyList<ILayoutFrame>
    {
        /// <inheritdoc/>
        public LayoutFrameReadOnlyList(LayoutFrameList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutFrame this[int index] { get { return (ILayoutFrame)base[index]; } }

        #region ILayoutFrame
        IEnumerator<ILayoutFrame> IEnumerable<ILayoutFrame>.GetEnumerator() { return ((IList<ILayoutFrame>)this).GetEnumerator(); }
        ILayoutFrame IReadOnlyList<ILayoutFrame>.this[int index] { get { return (ILayoutFrame)this[index]; } }
        #endregion
    }
}
