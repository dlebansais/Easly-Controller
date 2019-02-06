#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public interface IFocusVisibleCellViewList : IFrameVisibleCellViewList, IList<IFocusVisibleCellView>, IReadOnlyList<IFocusVisibleCellView>, IEqualComparable
    {
        new IFocusVisibleCellView this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusVisibleCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class FocusVisibleCellViewList : Collection<IFocusVisibleCellView>, IFocusVisibleCellViewList
    {
        #region Frame
        IFrameVisibleCellView IFrameVisibleCellViewList.this[int index] { get { return this[index]; } set { this[index] = (IFocusVisibleCellView)value; } }
        IFrameVisibleCellView IList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } set { this[index] = (IFocusVisibleCellView)value; } }
        int IList<IFrameVisibleCellView>.IndexOf(IFrameVisibleCellView value) { return IndexOf((IFocusVisibleCellView)value); }
        void IList<IFrameVisibleCellView>.Insert(int index, IFrameVisibleCellView item) { Insert(index, (IFocusVisibleCellView)item); }
        void ICollection<IFrameVisibleCellView>.Add(IFrameVisibleCellView item) { Add((IFocusVisibleCellView)item); }
        bool ICollection<IFrameVisibleCellView>.Contains(IFrameVisibleCellView value) { return Contains((IFocusVisibleCellView)value); }
        void ICollection<IFrameVisibleCellView>.CopyTo(IFrameVisibleCellView[] array, int index) { CopyTo((IFocusVisibleCellView[])array, index); }
        bool ICollection<IFrameVisibleCellView>.IsReadOnly { get { return ((ICollection<IFocusVisibleCellView>)this).IsReadOnly; } }
        bool ICollection<IFrameVisibleCellView>.Remove(IFrameVisibleCellView item) { return Remove((IFocusVisibleCellView)item); }
        IEnumerator<IFrameVisibleCellView> IEnumerable<IFrameVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        IFrameVisibleCellView IReadOnlyList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusVisibleCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusVisibleCellViewList AsVisibleCellViewList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsVisibleCellViewList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsVisibleCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
