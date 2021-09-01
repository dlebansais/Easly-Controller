#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyCollection<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>, IEqualComparable
    {
        public WriteablePlaceholderNodeStateReadOnlyList(WriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region IWriteablePlaceholderNodeState
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return new List<IWriteablePlaceholderNodeState>().GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteablePlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteablePlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
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
