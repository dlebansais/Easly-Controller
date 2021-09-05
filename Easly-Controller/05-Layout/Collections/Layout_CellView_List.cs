namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutCellViewList : FocusCellViewList, ICollection<ILayoutCellView>, IEnumerable<ILayoutCellView>, IList<ILayoutCellView>, IReadOnlyCollection<ILayoutCellView>, IReadOnlyList<ILayoutCellView>, IEqualComparable
    {
        #region ILayoutCellView
        void ICollection<ILayoutCellView>.Add(ILayoutCellView item) { Add(item); }
        bool ICollection<ILayoutCellView>.Contains(ILayoutCellView item) { return Contains(item); }
        void ICollection<ILayoutCellView>.CopyTo(ILayoutCellView[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutCellView>.Remove(ILayoutCellView item) { return Remove(item); }
        bool ICollection<ILayoutCellView>.IsReadOnly { get { return ((ICollection<IFocusCellView>)this).IsReadOnly; } }
        IEnumerator<ILayoutCellView> IEnumerable<ILayoutCellView>.GetEnumerator() { return ((IList<ILayoutCellView>)this).GetEnumerator(); }
        ILayoutCellView IList<ILayoutCellView>.this[int index] { get { return (ILayoutCellView)this[index]; } set { this[index] = value; } }
        int IList<ILayoutCellView>.IndexOf(ILayoutCellView item) { return IndexOf(item); }
        void IList<ILayoutCellView>.Insert(int index, ILayoutCellView item) { Insert(index, item); }
        ILayoutCellView IReadOnlyList<ILayoutCellView>.this[int index] { get { return (ILayoutCellView)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutCellViewList AsCellViewList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsCellViewList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override FrameCellViewReadOnlyList ToReadOnly()
        {
            return new LayoutCellViewReadOnlyList(this);
        }
    }
}
