namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusFrameReadOnlyList : FrameFrameReadOnlyList, IReadOnlyCollection<IFocusFrame>, IReadOnlyList<IFocusFrame>
    {
        /// <inheritdoc/>
        public FocusFrameReadOnlyList(FocusFrameList list)
            : base(list)
        {
        }

        #region IFocusFrame
        IEnumerator<IFocusFrame> IEnumerable<IFocusFrame>.GetEnumerator() { return ((IList<IFocusFrame>)this).GetEnumerator(); }
        IFocusFrame IReadOnlyList<IFocusFrame>.this[int index] { get { return (IFocusFrame)this[index]; } }
        #endregion
    }
}
