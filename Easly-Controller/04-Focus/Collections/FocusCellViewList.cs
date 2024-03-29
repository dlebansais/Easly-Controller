﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using Contracts;

    /// <inheritdoc/>
    public class FocusCellViewList : FrameCellViewList, ICollection<IFocusCellView>, IEnumerable<IFocusCellView>, IList<IFocusCellView>, IReadOnlyCollection<IFocusCellView>, IReadOnlyList<IFocusCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public new IFocusCellView this[int index] { get { return (IFocusCellView)base[index]; } set { base[index] = value; } }

        #region IFocusCellView
        void ICollection<IFocusCellView>.Add(IFocusCellView item) { Add(item); }
        bool ICollection<IFocusCellView>.Contains(IFocusCellView item) { return Contains(item); }
        void ICollection<IFocusCellView>.CopyTo(IFocusCellView[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusCellView>.Remove(IFocusCellView item) { return Remove(item); }
        bool ICollection<IFocusCellView>.IsReadOnly { get { return ((ICollection<IFrameCellView>)this).IsReadOnly; } }
        IEnumerator<IFocusCellView> IEnumerable<IFocusCellView>.GetEnumerator() { var iterator = ((List<IFrameCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusCellView)iterator.Current; } }
        IFocusCellView IList<IFocusCellView>.this[int index] { get { return (IFocusCellView)this[index]; } set { this[index] = value; } }
        int IList<IFocusCellView>.IndexOf(IFocusCellView item) { return IndexOf(item); }
        void IList<IFocusCellView>.Insert(int index, IFocusCellView item) { Insert(index, item); }
        IFocusCellView IReadOnlyList<IFocusCellView>.this[int index] { get { return (IFocusCellView)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameCellViewReadOnlyList ToReadOnly()
        {
            return new FocusCellViewReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusCellViewList AsOtherList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsOtherList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
