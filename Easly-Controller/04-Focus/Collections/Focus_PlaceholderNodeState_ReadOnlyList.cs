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
        new IFocusPlaceholderNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFocusPlaceholderNodeState value);
        new IEnumerator<IFocusPlaceholderNodeState> GetEnumerator();
        int IndexOf(IFocusPlaceholderNodeState value);
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
        bool IReadOnlyPlaceholderNodeStateReadOnlyList.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        int IReadOnlyPlaceholderNodeStateReadOnlyList.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteablePlaceholderNodeStateReadOnlyList.Contains(IWriteablePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteablePlaceholderNodeStateReadOnlyList.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFramePlaceholderNodeStateReadOnlyList.Contains(IFramePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFramePlaceholderNodeStateReadOnlyList.IndexOf(IFramePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusPlaceholderNodeStateReadOnlyList"/> objects.
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
