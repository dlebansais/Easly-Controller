namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutPlaceholderNodeStateReadOnlyList : FocusPlaceholderNodeStateReadOnlyList, IReadOnlyCollection<ILayoutPlaceholderNodeState>, IReadOnlyList<ILayoutPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public LayoutPlaceholderNodeStateReadOnlyList(LayoutPlaceholderNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutPlaceholderNodeState this[int index] { get { return (ILayoutPlaceholderNodeState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutPlaceholderNodeState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutPlaceholderNodeState)iterator.Current; } }

        #region ILayoutPlaceholderNodeState
        IEnumerator<ILayoutPlaceholderNodeState> IEnumerable<ILayoutPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        ILayoutPlaceholderNodeState IReadOnlyList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
