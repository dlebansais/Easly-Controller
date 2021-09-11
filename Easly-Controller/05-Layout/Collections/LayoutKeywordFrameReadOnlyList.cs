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

        /// <inheritdoc/>
        public new ILayoutKeywordFrame this[int index] { get { return (ILayoutKeywordFrame)base[index]; } }

        #region ILayoutKeywordFrame
        IEnumerator<ILayoutKeywordFrame> IEnumerable<ILayoutKeywordFrame>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutKeywordFrame)iterator.Current; } }
        ILayoutKeywordFrame IReadOnlyList<ILayoutKeywordFrame>.this[int index] { get { return (ILayoutKeywordFrame)this[index]; } }
        #endregion
    }
}
