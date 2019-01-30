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
        new int Count { get; }
        new IFocusVisibleCellView this[int index] { get; set; }
        new IEnumerator<IFocusVisibleCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class FocusVisibleCellViewList : Collection<IFocusVisibleCellView>, IFocusVisibleCellViewList
    {
        #region Frame
        public new IFrameVisibleCellView this[int index] { get { return base[index]; } set { base[index] = (IFocusVisibleCellView)value; } }
        public void Add(IFrameVisibleCellView item) { base.Add((IFocusVisibleCellView)item); }
        public void Insert(int index, IFrameVisibleCellView item) { base.Insert(index, (IFocusVisibleCellView)item); }
        public bool Remove(IFrameVisibleCellView item) { return base.Remove((IFocusVisibleCellView)item); }
        public void CopyTo(IFrameVisibleCellView[] array, int index) { base.CopyTo((IFocusVisibleCellView[])array, index); }
        bool ICollection<IFrameVisibleCellView>.IsReadOnly { get { return ((ICollection<IFocusVisibleCellView>)this).IsReadOnly; } }
        public bool Contains(IFrameVisibleCellView value) { return base.Contains((IFocusVisibleCellView)value); }
        public int IndexOf(IFrameVisibleCellView value) { return base.IndexOf((IFocusVisibleCellView)value); }
        public new IEnumerator<IFrameVisibleCellView> GetEnumerator() { return base.GetEnumerator(); }
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

            if (!(other is IFocusVisibleCellViewList AsVisibleCellViewList))
                return comparer.Failed();

            if (Count != AsVisibleCellViewList.Count)
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsVisibleCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
