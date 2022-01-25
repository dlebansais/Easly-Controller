namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

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
        /// <inheritdoc/>
        public new IEnumerator<ILayoutFrame> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFrame)iterator.Current; } }

        #region ILayoutFrame
        IEnumerator<ILayoutFrame> IEnumerable<ILayoutFrame>.GetEnumerator() { return GetEnumerator(); }
        ILayoutFrame IReadOnlyList<ILayoutFrame>.this[int index] { get { return (ILayoutFrame)this[index]; } }
        #endregion
    }
}
