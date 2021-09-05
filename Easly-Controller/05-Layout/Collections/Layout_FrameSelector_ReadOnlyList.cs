namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutFrameSelectorReadOnlyList : FocusFrameSelectorReadOnlyList, IReadOnlyCollection<ILayoutFrameSelector>, IReadOnlyList<ILayoutFrameSelector>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutFrameSelectorReadOnlyList(LayoutFrameSelectorList list)
            : base(list)
        {
        }

        #region ILayoutFrameSelector
        IEnumerator<ILayoutFrameSelector> IEnumerable<ILayoutFrameSelector>.GetEnumerator() { return ((IList<ILayoutFrameSelector>)this).GetEnumerator(); }
        ILayoutFrameSelector IReadOnlyList<ILayoutFrameSelector>.this[int index] { get { return (ILayoutFrameSelector)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutFrameSelectorReadOnlyList AsFrameSelectorReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsFrameSelectorReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsFrameSelectorReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
