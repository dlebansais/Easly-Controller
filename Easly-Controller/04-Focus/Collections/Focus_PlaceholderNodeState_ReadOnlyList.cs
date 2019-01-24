using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Focus
{
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
    public class FocusPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IFocusPlaceholderNodeState>, IFocusPlaceholderNodeStateReadOnlyList
    {
        public FocusPlaceholderNodeStateReadOnlyList(IFocusPlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateReadOnlyList.this[int index] { get { return base[index]; } }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return base[index]; } }
        public bool Contains(IWriteablePlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IWriteablePlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateReadOnlyList.this[int index] { get { return base[index]; } }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return base[index]; } }
        public bool Contains(IFramePlaceholderNodeState value) { return base.Contains((IFocusPlaceholderNodeState)value); }
        public int IndexOf(IFramePlaceholderNodeState value) { return base.IndexOf((IFocusPlaceholderNodeState)value); }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return base.GetEnumerator(); }
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

            if (!(other is IFocusPlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
                return comparer.Failed();

            if (Count != AsPlaceholderNodeStateReadOnlyList.Count)
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
