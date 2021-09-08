namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutFrameList : FocusFrameList, ICollection<ILayoutFrame>, IEnumerable<ILayoutFrame>, IList<ILayoutFrame>, IReadOnlyCollection<ILayoutFrame>, IReadOnlyList<ILayoutFrame>
    {
        /// <inheritdoc/>
        public new ILayoutFrame this[int index] { get { return (ILayoutFrame)base[index]; } set { base[index] = value; } }

        #region ILayoutFrame
        void ICollection<ILayoutFrame>.Add(ILayoutFrame item) { Add(item); }
        bool ICollection<ILayoutFrame>.Contains(ILayoutFrame item) { return Contains(item); }
        void ICollection<ILayoutFrame>.CopyTo(ILayoutFrame[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutFrame>.Remove(ILayoutFrame item) { return Remove(item); }
        bool ICollection<ILayoutFrame>.IsReadOnly { get { return ((ICollection<IFocusFrame>)this).IsReadOnly; } }
        IEnumerator<ILayoutFrame> IEnumerable<ILayoutFrame>.GetEnumerator() { return ((IList<ILayoutFrame>)this).GetEnumerator(); }
        ILayoutFrame IList<ILayoutFrame>.this[int index] { get { return (ILayoutFrame)this[index]; } set { this[index] = value; } }
        int IList<ILayoutFrame>.IndexOf(ILayoutFrame item) { return IndexOf(item); }
        void IList<ILayoutFrame>.Insert(int index, ILayoutFrame item) { Insert(index, item); }
        ILayoutFrame IReadOnlyList<ILayoutFrame>.this[int index] { get { return (ILayoutFrame)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameFrameReadOnlyList ToReadOnly()
        {
            return new LayoutFrameReadOnlyList(this);
        }
    }
}
