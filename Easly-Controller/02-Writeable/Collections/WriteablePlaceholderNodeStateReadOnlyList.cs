namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using Contracts;

    /// <inheritdoc/>
    public class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyCollection<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public WriteablePlaceholderNodeStateReadOnlyList(WriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IWriteablePlaceholderNodeState this[int index] { get { return (IWriteablePlaceholderNodeState)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteablePlaceholderNodeState)iterator.Current; } }

        #region IWriteablePlaceholderNodeState
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteablePlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
