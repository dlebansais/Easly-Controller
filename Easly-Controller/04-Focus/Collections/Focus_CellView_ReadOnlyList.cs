#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    public interface IFocusCellViewReadOnlyList : IFrameCellViewReadOnlyList, IReadOnlyList<IFocusCellView>, IEqualComparable
    {
        new IFocusCellView this[int index] { get; }
        new int Count { get; }
        bool Contains(IFocusCellView value);
        new IEnumerator<IFocusCellView> GetEnumerator();
        int IndexOf(IFocusCellView value);
    }

    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    internal class FocusCellViewReadOnlyList : ReadOnlyCollection<IFocusCellView>, IFocusCellViewReadOnlyList
    {
        public FocusCellViewReadOnlyList(IFocusCellViewList list)
            : base(list)
        {
        }

        #region Frame
        bool IFrameCellViewReadOnlyList.Contains(IFrameCellView value) { return Contains((IFocusCellView)value); }
        int IFrameCellViewReadOnlyList.IndexOf(IFrameCellView value) { return IndexOf((IFocusCellView)value); }
        IEnumerator<IFrameCellView> IEnumerable<IFrameCellView>.GetEnumerator() { return GetEnumerator(); }
        IFrameCellView IReadOnlyList<IFrameCellView>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellViewReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusCellViewReadOnlyList AsCellViewList))
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
