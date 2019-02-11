#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface ILayoutPlaceholderNodeStateList : IFocusPlaceholderNodeStateList, IList<ILayoutPlaceholderNodeState>, IReadOnlyList<ILayoutPlaceholderNodeState>
    {
        new ILayoutPlaceholderNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutPlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class LayoutPlaceholderNodeStateList : Collection<ILayoutPlaceholderNodeState>, ILayoutPlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        int IList<IReadOnlyPlaceholderNodeState>.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        void IList<IReadOnlyPlaceholderNodeState>.Insert(int index, IReadOnlyPlaceholderNodeState item) { Insert(index, (ILayoutPlaceholderNodeState)item); }
        void ICollection<IReadOnlyPlaceholderNodeState>.Add(IReadOnlyPlaceholderNodeState item) { Add((ILayoutPlaceholderNodeState)item); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        void ICollection<IReadOnlyPlaceholderNodeState>.CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { CopyTo((ILayoutPlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<ILayoutPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Remove(IReadOnlyPlaceholderNodeState item) { return Remove((ILayoutPlaceholderNodeState)item); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        int IList<IWriteablePlaceholderNodeState>.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        void IList<IWriteablePlaceholderNodeState>.Insert(int index, IWriteablePlaceholderNodeState item) { Insert(index, (ILayoutPlaceholderNodeState)item); }
        void ICollection<IWriteablePlaceholderNodeState>.Add(IWriteablePlaceholderNodeState item) { Add((ILayoutPlaceholderNodeState)item); }
        bool ICollection<IWriteablePlaceholderNodeState>.Contains(IWriteablePlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        void ICollection<IWriteablePlaceholderNodeState>.CopyTo(IWriteablePlaceholderNodeState[] array, int index) { CopyTo((ILayoutPlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<ILayoutPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteablePlaceholderNodeState>.Remove(IWriteablePlaceholderNodeState item) { return Remove((ILayoutPlaceholderNodeState)item); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        int IList<IFramePlaceholderNodeState>.IndexOf(IFramePlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        void IList<IFramePlaceholderNodeState>.Insert(int index, IFramePlaceholderNodeState item) { Insert(index, (ILayoutPlaceholderNodeState)item); }
        void ICollection<IFramePlaceholderNodeState>.Add(IFramePlaceholderNodeState item) { Add((ILayoutPlaceholderNodeState)item); }
        bool ICollection<IFramePlaceholderNodeState>.Contains(IFramePlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        void ICollection<IFramePlaceholderNodeState>.CopyTo(IFramePlaceholderNodeState[] array, int index) { CopyTo((ILayoutPlaceholderNodeState[])array, index); }
        bool ICollection<IFramePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<ILayoutPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IFramePlaceholderNodeState>.Remove(IFramePlaceholderNodeState item) { return Remove((ILayoutPlaceholderNodeState)item); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusPlaceholderNodeState IFocusPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        IEnumerator<IFocusPlaceholderNodeState> IFocusPlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFocusPlaceholderNodeState IList<IFocusPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutPlaceholderNodeState)value; } }
        int IList<IFocusPlaceholderNodeState>.IndexOf(IFocusPlaceholderNodeState value) { return IndexOf((ILayoutPlaceholderNodeState)value); }
        void IList<IFocusPlaceholderNodeState>.Insert(int index, IFocusPlaceholderNodeState item) { Insert(index, (ILayoutPlaceholderNodeState)item); }
        void ICollection<IFocusPlaceholderNodeState>.Add(IFocusPlaceholderNodeState item) { Add((ILayoutPlaceholderNodeState)item); }
        bool ICollection<IFocusPlaceholderNodeState>.Contains(IFocusPlaceholderNodeState value) { return Contains((ILayoutPlaceholderNodeState)value); }
        void ICollection<IFocusPlaceholderNodeState>.CopyTo(IFocusPlaceholderNodeState[] array, int index) { CopyTo((ILayoutPlaceholderNodeState[])array, index); }
        bool ICollection<IFocusPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<ILayoutPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IFocusPlaceholderNodeState>.Remove(IFocusPlaceholderNodeState item) { return Remove((ILayoutPlaceholderNodeState)item); }
        IEnumerator<IFocusPlaceholderNodeState> IEnumerable<IFocusPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFocusPlaceholderNodeState IReadOnlyList<IFocusPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new LayoutPlaceholderNodeStateReadOnlyList(this);
        }
    }
}
