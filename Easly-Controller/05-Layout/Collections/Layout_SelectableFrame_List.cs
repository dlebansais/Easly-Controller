namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutSelectableFrameList : FocusSelectableFrameList, ICollection<ILayoutSelectableFrame>, IEnumerable<ILayoutSelectableFrame>, IList<ILayoutSelectableFrame>, IReadOnlyCollection<ILayoutSelectableFrame>, IReadOnlyList<ILayoutSelectableFrame>
    {
        /// <inheritdoc/>
        public new ILayoutSelectableFrame this[int index] { get { return (ILayoutSelectableFrame)base[index]; } set { base[index] = value; } }

        #region ILayoutSelectableFrame
        void ICollection<ILayoutSelectableFrame>.Add(ILayoutSelectableFrame item) { Add(item); }
        bool ICollection<ILayoutSelectableFrame>.Contains(ILayoutSelectableFrame item) { return Contains(item); }
        void ICollection<ILayoutSelectableFrame>.CopyTo(ILayoutSelectableFrame[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutSelectableFrame>.Remove(ILayoutSelectableFrame item) { return Remove(item); }
        bool ICollection<ILayoutSelectableFrame>.IsReadOnly { get { return ((ICollection<IFocusSelectableFrame>)this).IsReadOnly; } }
        IEnumerator<ILayoutSelectableFrame> IEnumerable<ILayoutSelectableFrame>.GetEnumerator() { return ((IList<ILayoutSelectableFrame>)this).GetEnumerator(); }
        ILayoutSelectableFrame IList<ILayoutSelectableFrame>.this[int index] { get { return (ILayoutSelectableFrame)this[index]; } set { this[index] = value; } }
        int IList<ILayoutSelectableFrame>.IndexOf(ILayoutSelectableFrame item) { return IndexOf(item); }
        void IList<ILayoutSelectableFrame>.Insert(int index, ILayoutSelectableFrame item) { Insert(index, item); }
        ILayoutSelectableFrame IReadOnlyList<ILayoutSelectableFrame>.this[int index] { get { return (ILayoutSelectableFrame)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusSelectableFrameReadOnlyList ToReadOnly()
        {
            return new LayoutSelectableFrameReadOnlyList(this);
        }
    }
}
