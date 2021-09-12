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
        /// <inheritdoc/>
        public new IEnumerator<IFocusFrame> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFrameFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusFrame)iterator.Current; } }

        #region IFocusFrame
        IEnumerator<IFocusFrame> IEnumerable<IFocusFrame>.GetEnumerator() { return GetEnumerator(); }
        IFocusFrame IReadOnlyList<IFocusFrame>.this[int index] { get { return (IFocusFrame)this[index]; } }
        #endregion
    }
}
