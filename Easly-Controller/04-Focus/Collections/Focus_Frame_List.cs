namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusFrameList : FrameFrameList, ICollection<IFocusFrame>, IEnumerable<IFocusFrame>, IList<IFocusFrame>, IReadOnlyCollection<IFocusFrame>, IReadOnlyList<IFocusFrame>
    {
        /// <inheritdoc/>
        public new IFocusFrame this[int index] { get { return (IFocusFrame)base[index]; } set { base[index] = value; } }

        #region IFocusFrame
        void ICollection<IFocusFrame>.Add(IFocusFrame item) { Add(item); }
        bool ICollection<IFocusFrame>.Contains(IFocusFrame item) { return Contains(item); }
        void ICollection<IFocusFrame>.CopyTo(IFocusFrame[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusFrame>.Remove(IFocusFrame item) { return Remove(item); }
        bool ICollection<IFocusFrame>.IsReadOnly { get { return ((ICollection<IFrameFrame>)this).IsReadOnly; } }
        IEnumerator<IFocusFrame> IEnumerable<IFocusFrame>.GetEnumerator() { return ((IList<IFocusFrame>)this).GetEnumerator(); }
        IFocusFrame IList<IFocusFrame>.this[int index] { get { return (IFocusFrame)this[index]; } set { this[index] = value; } }
        int IList<IFocusFrame>.IndexOf(IFocusFrame item) { return IndexOf(item); }
        void IList<IFocusFrame>.Insert(int index, IFocusFrame item) { Insert(index, item); }
        IFocusFrame IReadOnlyList<IFocusFrame>.this[int index] { get { return (IFocusFrame)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameFrameReadOnlyList ToReadOnly()
        {
            return new FocusFrameReadOnlyList(this);
        }
    }
}
