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

        /// <inheritdoc/>
        public new IFocusKeywordFrame this[int index] { get { return (IFocusKeywordFrame)base[index]; } }

        #region IFocusKeywordFrame
        IEnumerator<IFocusKeywordFrame> IEnumerable<IFocusKeywordFrame>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusKeywordFrame)iterator.Current; } }
        IFocusKeywordFrame IReadOnlyList<IFocusKeywordFrame>.this[int index] { get { return (IFocusKeywordFrame)this[index]; } }
        #endregion
    }
}
