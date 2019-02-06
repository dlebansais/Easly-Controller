#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFocusPlaceholderNodeStateList : IFramePlaceholderNodeStateList, IList<IFocusPlaceholderNodeState>, IReadOnlyList<IFocusPlaceholderNodeState>
    {
        new IFocusPlaceholderNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusPlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class FocusPlaceholderNodeStateList : Collection<IFocusPlaceholderNodeState>, IFocusPlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        int IList<IReadOnlyPlaceholderNodeState>.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        void IList<IReadOnlyPlaceholderNodeState>.Insert(int index, IReadOnlyPlaceholderNodeState item) { Insert(index, (IFocusPlaceholderNodeState)item); }
        void ICollection<IReadOnlyPlaceholderNodeState>.Add(IReadOnlyPlaceholderNodeState item) { Add((IFocusPlaceholderNodeState)item); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        void ICollection<IReadOnlyPlaceholderNodeState>.CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Remove(IReadOnlyPlaceholderNodeState item) { return Remove((IFocusPlaceholderNodeState)item); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        int IList<IWriteablePlaceholderNodeState>.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        void IList<IWriteablePlaceholderNodeState>.Insert(int index, IWriteablePlaceholderNodeState item) { Insert(index, (IFocusPlaceholderNodeState)item); }
        void ICollection<IWriteablePlaceholderNodeState>.Add(IWriteablePlaceholderNodeState item) { Add((IFocusPlaceholderNodeState)item); }
        bool ICollection<IWriteablePlaceholderNodeState>.Contains(IWriteablePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        void ICollection<IWriteablePlaceholderNodeState>.CopyTo(IWriteablePlaceholderNodeState[] array, int index) { CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteablePlaceholderNodeState>.Remove(IWriteablePlaceholderNodeState item) { return Remove((IFocusPlaceholderNodeState)item); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFramePlaceholderNodeState IFramePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        IEnumerator<IFramePlaceholderNodeState> IFramePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusPlaceholderNodeState)value; } }
        int IList<IFramePlaceholderNodeState>.IndexOf(IFramePlaceholderNodeState value) { return IndexOf((IFocusPlaceholderNodeState)value); }
        void IList<IFramePlaceholderNodeState>.Insert(int index, IFramePlaceholderNodeState item) { Insert(index, (IFocusPlaceholderNodeState)item); }
        void ICollection<IFramePlaceholderNodeState>.Add(IFramePlaceholderNodeState item) { Add((IFocusPlaceholderNodeState)item); }
        bool ICollection<IFramePlaceholderNodeState>.Contains(IFramePlaceholderNodeState value) { return Contains((IFocusPlaceholderNodeState)value); }
        void ICollection<IFramePlaceholderNodeState>.CopyTo(IFramePlaceholderNodeState[] array, int index) { CopyTo((IFocusPlaceholderNodeState[])array, index); }
        bool ICollection<IFramePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IFramePlaceholderNodeState>.Remove(IFramePlaceholderNodeState item) { return Remove((IFocusPlaceholderNodeState)item); }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new FocusPlaceholderNodeStateReadOnlyList(this);
        }
    }
}
