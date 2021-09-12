namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusVisibleCellViewList : FrameVisibleCellViewList, ICollection<IFocusVisibleCellView>, IEnumerable<IFocusVisibleCellView>, IList<IFocusVisibleCellView>, IReadOnlyCollection<IFocusVisibleCellView>, IReadOnlyList<IFocusVisibleCellView>
    {
        /// <inheritdoc/>
        public new IFocusVisibleCellView this[int index] { get { return (IFocusVisibleCellView)base[index]; } set { base[index] = value; } }

        #region IFocusVisibleCellView
        void ICollection<IFocusVisibleCellView>.Add(IFocusVisibleCellView item) { Add(item); }
        bool ICollection<IFocusVisibleCellView>.Contains(IFocusVisibleCellView item) { return Contains(item); }
        void ICollection<IFocusVisibleCellView>.CopyTo(IFocusVisibleCellView[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusVisibleCellView>.Remove(IFocusVisibleCellView item) { return Remove(item); }
        bool ICollection<IFocusVisibleCellView>.IsReadOnly { get { return ((ICollection<IFrameVisibleCellView>)this).IsReadOnly; } }
        IEnumerator<IFocusVisibleCellView> IEnumerable<IFocusVisibleCellView>.GetEnumerator() { var iterator = ((List<IFrameVisibleCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusVisibleCellView)iterator.Current; } }
        IFocusVisibleCellView IList<IFocusVisibleCellView>.this[int index] { get { return (IFocusVisibleCellView)this[index]; } set { this[index] = value; } }
        int IList<IFocusVisibleCellView>.IndexOf(IFocusVisibleCellView item) { return IndexOf(item); }
        void IList<IFocusVisibleCellView>.Insert(int index, IFocusVisibleCellView item) { Insert(index, item); }
        IFocusVisibleCellView IReadOnlyList<IFocusVisibleCellView>.this[int index] { get { return (IFocusVisibleCellView)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameVisibleCellViewReadOnlyList ToReadOnly()
        {
            return new FocusVisibleCellViewReadOnlyList(this);
        }
    }
}
