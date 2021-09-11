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

        /// <inheritdoc/>
        public new IFocusFrame this[int index] { get { return (IFocusFrame)base[index]; } }

        #region IFocusFrame
        IEnumerator<IFocusFrame> IEnumerable<IFocusFrame>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusFrame)iterator.Current; } }
        IFocusFrame IReadOnlyList<IFocusFrame>.this[int index] { get { return (IFocusFrame)this[index]; } }
        #endregion
    }
}
