#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFramePlaceholderNodeStateReadOnlyList : IWriteablePlaceholderNodeStateReadOnlyList, IReadOnlyList<IFramePlaceholderNodeState>
    {
        new IFramePlaceholderNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFramePlaceholderNodeState value);
        new IEnumerator<IFramePlaceholderNodeState> GetEnumerator();
        int IndexOf(IFramePlaceholderNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    internal class FramePlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IFramePlaceholderNodeState>, IFramePlaceholderNodeStateReadOnlyList
    {
        public FramePlaceholderNodeStateReadOnlyList(IFramePlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyPlaceholderNodeStateReadOnlyList.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IFramePlaceholderNodeState)value); }
        int IReadOnlyPlaceholderNodeStateReadOnlyList.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IFramePlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteablePlaceholderNodeStateReadOnlyList.Contains(IWriteablePlaceholderNodeState value) { return Contains((IFramePlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteablePlaceholderNodeStateReadOnlyList.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((IFramePlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFramePlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
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
