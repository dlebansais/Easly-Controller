namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FramePlaceholderNodeStateReadOnlyList : WriteablePlaceholderNodeStateReadOnlyList, IReadOnlyCollection<IFramePlaceholderNodeState>, IReadOnlyList<IFramePlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public FramePlaceholderNodeStateReadOnlyList(FramePlaceholderNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFramePlaceholderNodeState this[int index] { get { return (IFramePlaceholderNodeState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFramePlaceholderNodeState> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFramePlaceholderNodeState)iterator.Current; } }

        #region IFramePlaceholderNodeState
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return (IFramePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
