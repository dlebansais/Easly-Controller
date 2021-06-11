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
    public interface IWriteablePlaceholderNodeStateReadOnlyList : IReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new IWriteablePlaceholderNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IWriteablePlaceholderNodeState value);
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
        int IndexOf(IWriteablePlaceholderNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    internal class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateReadOnlyList
    {
        public WriteablePlaceholderNodeStateReadOnlyList(IWriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyPlaceholderNodeStateReadOnlyList.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IWriteablePlaceholderNodeState)value); }
        int IReadOnlyPlaceholderNodeStateReadOnlyList.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IWriteablePlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteablePlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
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
