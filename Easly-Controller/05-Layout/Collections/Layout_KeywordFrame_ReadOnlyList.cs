namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutKeywordFrameReadOnlyList : FocusKeywordFrameReadOnlyList, IReadOnlyCollection<ILayoutKeywordFrame>, IReadOnlyList<ILayoutKeywordFrame>
    {
        /// <inheritdoc/>
        public LayoutKeywordFrameReadOnlyList(LayoutKeywordFrameList list)
            : base(list)
        {
        }

        #region ILayoutKeywordFrame
        IEnumerator<ILayoutKeywordFrame> IEnumerable<ILayoutKeywordFrame>.GetEnumerator() { return ((IList<ILayoutKeywordFrame>)this).GetEnumerator(); }
        ILayoutKeywordFrame IReadOnlyList<ILayoutKeywordFrame>.this[int index] { get { return (ILayoutKeywordFrame)this[index]; } }
        #endregion
    }
}
