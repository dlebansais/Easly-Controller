namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

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

        #region IWriteablePlaceholderNodeState
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteablePlaceholderNodeState)iterator.Current; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteablePlaceholderNodeStateReadOnlyList AsOtherReadOnlyList))
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
