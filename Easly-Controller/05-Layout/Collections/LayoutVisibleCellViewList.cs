namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutVisibleCellViewList : FocusVisibleCellViewList, ICollection<ILayoutVisibleCellView>, IEnumerable<ILayoutVisibleCellView>, IList<ILayoutVisibleCellView>, IReadOnlyCollection<ILayoutVisibleCellView>, IReadOnlyList<ILayoutVisibleCellView>
    {
        /// <inheritdoc/>
        public new ILayoutVisibleCellView this[int index] { get { return (ILayoutVisibleCellView)base[index]; } set { base[index] = value; } }

        #region ILayoutVisibleCellView
        void ICollection<ILayoutVisibleCellView>.Add(ILayoutVisibleCellView item) { Add(item); }
        bool ICollection<ILayoutVisibleCellView>.Contains(ILayoutVisibleCellView item) { return Contains(item); }
        void ICollection<ILayoutVisibleCellView>.CopyTo(ILayoutVisibleCellView[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutVisibleCellView>.Remove(ILayoutVisibleCellView item) { return Remove(item); }
        bool ICollection<ILayoutVisibleCellView>.IsReadOnly { get { return ((ICollection<IFocusVisibleCellView>)this).IsReadOnly; } }
        IEnumerator<ILayoutVisibleCellView> IEnumerable<ILayoutVisibleCellView>.GetEnumerator() { var iterator = ((List<IFrameVisibleCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutVisibleCellView)iterator.Current; } }
        ILayoutVisibleCellView IList<ILayoutVisibleCellView>.this[int index] { get { return (ILayoutVisibleCellView)this[index]; } set { this[index] = value; } }
        int IList<ILayoutVisibleCellView>.IndexOf(ILayoutVisibleCellView item) { return IndexOf(item); }
        void IList<ILayoutVisibleCellView>.Insert(int index, ILayoutVisibleCellView item) { Insert(index, item); }
        ILayoutVisibleCellView IReadOnlyList<ILayoutVisibleCellView>.this[int index] { get { return (ILayoutVisibleCellView)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameVisibleCellViewReadOnlyList ToReadOnly()
        {
            return new LayoutVisibleCellViewReadOnlyList(this);
        }
    }
}
