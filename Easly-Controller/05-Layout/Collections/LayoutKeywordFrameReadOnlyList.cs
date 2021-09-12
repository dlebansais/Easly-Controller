namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutKeywordFrameReadOnlyList : FocusKeywordFrameReadOnlyList, IReadOnlyCollection<ILayoutKeywordFrame>, IReadOnlyList<ILayoutKeywordFrame>
    {
        /// <inheritdoc/>
        public LayoutKeywordFrameReadOnlyList(LayoutKeywordFrameList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutKeywordFrame this[int index] { get { return (ILayoutKeywordFrame)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutKeywordFrame> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFrameKeywordFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutKeywordFrame)iterator.Current; } }

        #region ILayoutKeywordFrame
        IEnumerator<ILayoutKeywordFrame> IEnumerable<ILayoutKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        ILayoutKeywordFrame IReadOnlyList<ILayoutKeywordFrame>.this[int index] { get { return (ILayoutKeywordFrame)this[index]; } }
        #endregion
    }
}
