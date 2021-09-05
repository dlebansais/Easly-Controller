namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutPlaceholderNodeStateReadOnlyList : FocusPlaceholderNodeStateReadOnlyList, IReadOnlyCollection<ILayoutPlaceholderNodeState>, IReadOnlyList<ILayoutPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutPlaceholderNodeStateReadOnlyList(LayoutPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ILayoutPlaceholderNodeState
        IEnumerator<ILayoutPlaceholderNodeState> IEnumerable<ILayoutPlaceholderNodeState>.GetEnumerator() { return ((IList<ILayoutPlaceholderNodeState>)this).GetEnumerator(); }
        ILayoutPlaceholderNodeState IReadOnlyList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsPlaceholderNodeStateReadOnlyList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
