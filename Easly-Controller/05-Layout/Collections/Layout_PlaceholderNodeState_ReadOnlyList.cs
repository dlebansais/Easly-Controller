#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface ILayoutPlaceholderNodeStateReadOnlyList : IFocusPlaceholderNodeStateReadOnlyList, IReadOnlyList<ILayoutPlaceholderNodeState>
    {
        new ILayoutPlaceholderNodeState this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutPlaceholderNodeState value);
        new IEnumerator<ILayoutPlaceholderNodeState> GetEnumerator();
        int IndexOf(ILayoutPlaceholderNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    internal class LayoutPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<ILayoutPlaceholderNodeState>, ILayoutPlaceholderNodeStateReadOnlyList
    {
        public LayoutPlaceholderNodeStateReadOnlyList(ILayoutPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyPlaceholderNodeStateReadOnlyList.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        int IReadOnlyPlaceholderNodeStateReadOnlyList.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteablePlaceholderNodeStateReadOnlyList.Contains(IWriteablePlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteablePlaceholderNodeStateReadOnlyList.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFramePlaceholderNodeStateReadOnlyList.Contains(IFramePlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFramePlaceholderNodeStateReadOnlyList.IndexOf(IFramePlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusPlaceholderNodeState IFocusPlaceholderNodeStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFocusPlaceholderNodeStateReadOnlyList.Contains(IFocusPlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IFocusPlaceholderNodeState> IFocusPlaceholderNodeStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFocusPlaceholderNodeStateReadOnlyList.IndexOf(IFocusPlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        IEnumerator<IFocusPlaceholderNodeState> IEnumerable<IFocusPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusPlaceholderNodeState IReadOnlyList<IFocusPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutPlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
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
