namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutNodeFrameVisibilityList : FocusNodeFrameVisibilityList, ICollection<ILayoutNodeFrameVisibility>, IEnumerable<ILayoutNodeFrameVisibility>, IList<ILayoutNodeFrameVisibility>, IReadOnlyCollection<ILayoutNodeFrameVisibility>, IReadOnlyList<ILayoutNodeFrameVisibility>
    {
        #region ILayoutNodeFrameVisibility
        void ICollection<ILayoutNodeFrameVisibility>.Add(ILayoutNodeFrameVisibility item) { Add(item); }
        bool ICollection<ILayoutNodeFrameVisibility>.Contains(ILayoutNodeFrameVisibility item) { return Contains(item); }
        void ICollection<ILayoutNodeFrameVisibility>.CopyTo(ILayoutNodeFrameVisibility[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutNodeFrameVisibility>.Remove(ILayoutNodeFrameVisibility item) { return Remove(item); }
        bool ICollection<ILayoutNodeFrameVisibility>.IsReadOnly { get { return ((ICollection<IFocusNodeFrameVisibility>)this).IsReadOnly; } }
        IEnumerator<ILayoutNodeFrameVisibility> IEnumerable<ILayoutNodeFrameVisibility>.GetEnumerator() { return ((IList<ILayoutNodeFrameVisibility>)this).GetEnumerator(); }
        ILayoutNodeFrameVisibility IList<ILayoutNodeFrameVisibility>.this[int index] { get { return (ILayoutNodeFrameVisibility)this[index]; } set { this[index] = value; } }
        int IList<ILayoutNodeFrameVisibility>.IndexOf(ILayoutNodeFrameVisibility item) { return IndexOf(item); }
        void IList<ILayoutNodeFrameVisibility>.Insert(int index, ILayoutNodeFrameVisibility item) { Insert(index, item); }
        ILayoutNodeFrameVisibility IReadOnlyList<ILayoutNodeFrameVisibility>.this[int index] { get { return (ILayoutNodeFrameVisibility)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusNodeFrameVisibilityReadOnlyList ToReadOnly()
        {
            return new LayoutNodeFrameVisibilityReadOnlyList(this);
        }
    }
}
