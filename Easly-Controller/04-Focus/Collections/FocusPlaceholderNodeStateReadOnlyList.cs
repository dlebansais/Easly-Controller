namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusPlaceholderNodeStateReadOnlyList : FramePlaceholderNodeStateReadOnlyList, IReadOnlyCollection<IFocusPlaceholderNodeState>, IReadOnlyList<IFocusPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public FocusPlaceholderNodeStateReadOnlyList(FocusPlaceholderNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusPlaceholderNodeState this[int index] { get { return (IFocusPlaceholderNodeState)base[index]; } }

        #region IFocusPlaceholderNodeState
        IEnumerator<IFocusPlaceholderNodeState> IEnumerable<IFocusPlaceholderNodeState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusPlaceholderNodeState)iterator.Current; } }
        IFocusPlaceholderNodeState IReadOnlyList<IFocusPlaceholderNodeState>.this[int index] { get { return (IFocusPlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusPlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
