#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface ILayoutCellViewList : IFocusCellViewList, IList<ILayoutCellView>, IReadOnlyList<ILayoutCellView>, IEqualComparable
    {
        new ILayoutCellView this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutCellView> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    internal class LayoutCellViewList : Collection<ILayoutCellView>, ILayoutCellViewList
    {
        #region Frame
        IFrameCellView IFrameCellViewList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCellView)value; } }
        IFrameCellView IList<IFrameCellView>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCellView)value; } }
        int IList<IFrameCellView>.IndexOf(IFrameCellView value) { return IndexOf((ILayoutCellView)value); }
        void IList<IFrameCellView>.Insert(int index, IFrameCellView item) { Insert(index, (ILayoutCellView)item); }
        void ICollection<IFrameCellView>.Add(IFrameCellView item) { Add((ILayoutCellView)item); }
        bool ICollection<IFrameCellView>.Contains(IFrameCellView value) { return Contains((ILayoutCellView)value); }
        void ICollection<IFrameCellView>.CopyTo(IFrameCellView[] array, int index) { CopyTo((ILayoutCellView[])array, index); }
        bool ICollection<IFrameCellView>.IsReadOnly { get { return ((ICollection<ILayoutCellView>)this).IsReadOnly; } }
        bool ICollection<IFrameCellView>.Remove(IFrameCellView item) { return Remove((ILayoutCellView)item); }
        IEnumerator<IFrameCellView> IEnumerable<IFrameCellView>.GetEnumerator() { return GetEnumerator(); }
        IFrameCellView IReadOnlyList<IFrameCellView>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusCellView IFocusCellViewList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCellView)value; } }
        IEnumerator<IFocusCellView> IFocusCellViewList.GetEnumerator() { return GetEnumerator(); }
        IFocusCellView IList<IFocusCellView>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutCellView)value; } }
        int IList<IFocusCellView>.IndexOf(IFocusCellView value) { return IndexOf((ILayoutCellView)value); }
        void IList<IFocusCellView>.Insert(int index, IFocusCellView item) { Insert(index, (ILayoutCellView)item); }
        void ICollection<IFocusCellView>.Add(IFocusCellView item) { Add((ILayoutCellView)item); }
        bool ICollection<IFocusCellView>.Contains(IFocusCellView value) { return Contains((ILayoutCellView)value); }
        void ICollection<IFocusCellView>.CopyTo(IFocusCellView[] array, int index) { CopyTo((ILayoutCellView[])array, index); }
        bool ICollection<IFocusCellView>.IsReadOnly { get { return ((ICollection<ILayoutCellView>)this).IsReadOnly; } }
        bool ICollection<IFocusCellView>.Remove(IFocusCellView item) { return Remove((ILayoutCellView)item); }
        IEnumerator<IFocusCellView> IEnumerable<IFocusCellView>.GetEnumerator() { return GetEnumerator(); }
        IFocusCellView IReadOnlyList<IFocusCellView>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
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
    }
}
