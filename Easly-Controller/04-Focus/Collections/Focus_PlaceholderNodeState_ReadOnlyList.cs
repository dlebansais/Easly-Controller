#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFocusPlaceholderNodeStateReadOnlyList : IFramePlaceholderNodeStateReadOnlyList, IReadOnlyList<IFocusPlaceholderNodeState>
    {
        new int Count { get; }
        new IFocusPlaceholderNodeState this[int index] { get; }
        bool Contains(IFocusPlaceholderNodeState value);
        int IndexOf(IFocusPlaceholderNodeState value);
        new IEnumerator<IFocusPlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    internal class FocusPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IFocusPlaceholderNodeState>, IFocusPlaceholderNodeStateReadOnlyList
    {
        public FocusPlaceholderNodeStateReadOnlyList(IFocusPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        bool IReadOnlyPlaceholderNodeStateReadOnlyList.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        int IReadOnlyPlaceholderNodeStateReadOnlyList.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        bool IWriteablePlaceholderNodeStateReadOnlyList.Contains(IWriteablePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        int IWriteablePlaceholderNodeStateReadOnlyList.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        bool IFramePlaceholderNodeStateReadOnlyList.Contains(IFramePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        int IFramePlaceholderNodeStateReadOnlyList.IndexOf(IFramePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusPlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
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
