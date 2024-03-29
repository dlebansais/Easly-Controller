﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using Contracts;

    /// <inheritdoc/>
    public class FocusCellViewReadOnlyList : FrameCellViewReadOnlyList, IReadOnlyCollection<IFocusCellView>, IReadOnlyList<IFocusCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusCellViewReadOnlyList(FocusCellViewList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusCellView this[int index] { get { return (IFocusCellView)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusCellView> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFrameCellView>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusCellView)iterator.Current; } }

        #region IFocusCellView
        IEnumerator<IFocusCellView> IEnumerable<IFocusCellView>.GetEnumerator() { return GetEnumerator(); }
        IFocusCellView IReadOnlyList<IFocusCellView>.this[int index] { get { return (IFocusCellView)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusCellViewReadOnlyList AsOtherReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsOtherReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
