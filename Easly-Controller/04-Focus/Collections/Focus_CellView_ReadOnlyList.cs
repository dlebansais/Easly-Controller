﻿namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusCellViewReadOnlyList : FrameCellViewReadOnlyList, IReadOnlyCollection<IFocusCellView>, IReadOnlyList<IFocusCellView>
    {
        /// <inheritdoc/>
        public FocusCellViewReadOnlyList(FocusCellViewList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusCellView this[int index] { get { return (IFocusCellView)base[index]; } }

        #region IFocusCellView
        IEnumerator<IFocusCellView> IEnumerable<IFocusCellView>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusCellView)iterator.Current; } }
        IFocusCellView IReadOnlyList<IFocusCellView>.this[int index] { get { return (IFocusCellView)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusCellViewReadOnlyList AsCellViewReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsCellViewReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
