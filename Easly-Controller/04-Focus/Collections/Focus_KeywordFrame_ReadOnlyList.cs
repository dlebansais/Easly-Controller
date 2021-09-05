namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusKeywordFrameReadOnlyList : FrameKeywordFrameReadOnlyList, IReadOnlyCollection<IFocusKeywordFrame>, IReadOnlyList<IFocusKeywordFrame>
    {
        /// <inheritdoc/>
        public FocusKeywordFrameReadOnlyList(FocusKeywordFrameList list)
            : base(list)
        {
        }

        #region IFocusKeywordFrame
        IEnumerator<IFocusKeywordFrame> IEnumerable<IFocusKeywordFrame>.GetEnumerator() { return ((IList<IFocusKeywordFrame>)this).GetEnumerator(); }
        IFocusKeywordFrame IReadOnlyList<IFocusKeywordFrame>.this[int index] { get { return (IFocusKeywordFrame)this[index]; } }
        #endregion
    }
}
