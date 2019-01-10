using EaslyController.Frame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface IFocusCellViewList : IFrameCellViewList, IList<IFocusCellView>, IReadOnlyList<IFocusCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFocusCellView this[int index] { get; set; }
        new IEnumerator<IFocusCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public class FocusCellViewList : Collection<IFocusCellView>, IFocusCellViewList
    {
        #region Frame
        public new IFrameCellView this[int index] { get { return base[index]; } set { base[index] = (IFocusCellView)value; } }
        public void Add(IFrameCellView item) { base.Add((IFocusCellView)item); }
        public void Insert(int index, IFrameCellView item) { base.Insert(index, (IFocusCellView)item); }
        public bool Remove(IFrameCellView item) { return base.Remove((IFocusCellView)item); }
        public void CopyTo(IFrameCellView[] array, int index) { base.CopyTo((IFocusCellView[])array, index); }
        bool ICollection<IFrameCellView>.IsReadOnly { get { return ((ICollection<IFocusCellView>)this).IsReadOnly; } }
        public bool Contains(IFrameCellView value) { return base.Contains((IFocusCellView)value); }
        public int IndexOf(IFrameCellView value) { return base.IndexOf((IFocusCellView)value); }
        public new IEnumerator<IFrameCellView> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusCellViewList AsCellViewList))
                return false;

            if (Count != AsCellViewList.Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
