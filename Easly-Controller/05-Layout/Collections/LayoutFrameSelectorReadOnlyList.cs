namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using Contracts;

    /// <inheritdoc/>
    public class LayoutFrameSelectorReadOnlyList : FocusFrameSelectorReadOnlyList, IReadOnlyCollection<ILayoutFrameSelector>, IReadOnlyList<ILayoutFrameSelector>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutFrameSelectorReadOnlyList(LayoutFrameSelectorList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutFrameSelector this[int index] { get { return (ILayoutFrameSelector)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutFrameSelector> GetEnumerator() { var iterator = ((ReadOnlyCollection<IFocusFrameSelector>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFrameSelector)iterator.Current; } }

        #region ILayoutFrameSelector
        IEnumerator<ILayoutFrameSelector> IEnumerable<ILayoutFrameSelector>.GetEnumerator() { return GetEnumerator(); }
        ILayoutFrameSelector IReadOnlyList<ILayoutFrameSelector>.this[int index] { get { return (ILayoutFrameSelector)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutFrameSelectorReadOnlyList AsOtherReadOnlyList))
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
