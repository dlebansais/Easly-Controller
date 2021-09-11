namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

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

        #region ILayoutFrameSelector
        IEnumerator<ILayoutFrameSelector> IEnumerable<ILayoutFrameSelector>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutFrameSelector)iterator.Current; } }
        ILayoutFrameSelector IReadOnlyList<ILayoutFrameSelector>.this[int index] { get { return (ILayoutFrameSelector)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutFrameSelectorReadOnlyList AsOtherReadOnlyList))
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
