#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface IFocusCellViewList : IFrameCellViewList, IList<IFocusCellView>, IReadOnlyList<IFocusCellView>, IEqualComparable
    {
        new IFocusCellView this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    internal class FocusCellViewList : Collection<IFocusCellView>, IFocusCellViewList
    {
        #region Frame
        IFrameCellView IFrameCellViewList.this[int index] { get { return this[index]; } set { this[index] = (IFocusCellView)value; } }
        IEnumerator<IFrameCellView> IFrameCellViewList.GetEnumerator() { return GetEnumerator(); }
        IFrameCellView IList<IFrameCellView>.this[int index] { get { return this[index]; } set { this[index] = (IFocusCellView)value; } }
        int IList<IFrameCellView>.IndexOf(IFrameCellView value) { return IndexOf((IFocusCellView)value); }
        void IList<IFrameCellView>.Insert(int index, IFrameCellView item) { Insert(index, (IFocusCellView)item); }
        void ICollection<IFrameCellView>.Add(IFrameCellView item) { Add((IFocusCellView)item); }
        bool ICollection<IFrameCellView>.Contains(IFrameCellView value) { return Contains((IFocusCellView)value); }
        void ICollection<IFrameCellView>.CopyTo(IFrameCellView[] array, int index) { CopyTo((IFocusCellView[])array, index); }
        bool ICollection<IFrameCellView>.IsReadOnly { get { return ((ICollection<IFocusCellView>)this).IsReadOnly; } }
        bool ICollection<IFrameCellView>.Remove(IFrameCellView item) { return Remove((IFocusCellView)item); }
        IEnumerator<IFrameCellView> IEnumerable<IFrameCellView>.GetEnumerator() { return GetEnumerator(); }
        IFrameCellView IReadOnlyList<IFrameCellView>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IFrameCellViewReadOnlyList ToReadOnly()
        {
            return new FocusCellViewReadOnlyList(this);
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusCellViewList AsCellViewList))
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
