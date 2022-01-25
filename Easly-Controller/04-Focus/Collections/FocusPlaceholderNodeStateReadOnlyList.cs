namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using Contracts;

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
        /// <inheritdoc/>
        public new IEnumerator<IFocusPlaceholderNodeState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusPlaceholderNodeState)iterator.Current; } }

        #region IFocusPlaceholderNodeState
        IEnumerator<IFocusPlaceholderNodeState> IEnumerable<IFocusPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusPlaceholderNodeState IReadOnlyList<IFocusPlaceholderNodeState>.this[int index] { get { return (IFocusPlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusPlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
