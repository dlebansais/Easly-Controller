namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// <inheritdoc/>
        public new IEnumerator<IFocusKeywordFrame> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameKeywordFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusKeywordFrame)iterator.Current; } }

        #region IFocusKeywordFrame
        IEnumerator<IFocusKeywordFrame> IEnumerable<IFocusKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        IFocusKeywordFrame IReadOnlyList<IFocusKeywordFrame>.this[int index] { get { return (IFocusKeywordFrame)this[index]; } }
        #endregion
    }
}
