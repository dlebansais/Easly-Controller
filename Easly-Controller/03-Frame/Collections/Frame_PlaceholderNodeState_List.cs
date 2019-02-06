#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IFramePlaceholderNodeStateList : IWriteablePlaceholderNodeStateList, IList<IFramePlaceholderNodeState>, IReadOnlyList<IFramePlaceholderNodeState>
    {
        new int Count { get; }
        new IFramePlaceholderNodeState this[int index] { get; set; }
        new IEnumerator<IFramePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class FramePlaceholderNodeStateList : Collection<IFramePlaceholderNodeState>, IFramePlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        int IList<IReadOnlyPlaceholderNodeState>.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IFramePlaceholderNodeState)value); }
        void IList<IReadOnlyPlaceholderNodeState>.Insert(int index, IReadOnlyPlaceholderNodeState item) { Insert(index, (IFramePlaceholderNodeState)item); }
        void ICollection<IReadOnlyPlaceholderNodeState>.Add(IReadOnlyPlaceholderNodeState item) { Add((IFramePlaceholderNodeState)item); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IFramePlaceholderNodeState)value); }
        void ICollection<IReadOnlyPlaceholderNodeState>.CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Remove(IReadOnlyPlaceholderNodeState item) { return Remove((IFramePlaceholderNodeState)item); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        IEnumerator<IWriteablePlaceholderNodeState> IWriteablePlaceholderNodeStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IFramePlaceholderNodeState)value; } }
        int IList<IWriteablePlaceholderNodeState>.IndexOf(IWriteablePlaceholderNodeState value) { return IndexOf((IFramePlaceholderNodeState)value); }
        void IList<IWriteablePlaceholderNodeState>.Insert(int index, IWriteablePlaceholderNodeState item) { Insert(index, (IFramePlaceholderNodeState)item); }
        void ICollection<IWriteablePlaceholderNodeState>.Add(IWriteablePlaceholderNodeState item) { Add((IFramePlaceholderNodeState)item); }
        bool ICollection<IWriteablePlaceholderNodeState>.Contains(IWriteablePlaceholderNodeState value) { return Contains((IFramePlaceholderNodeState)value); }
        void ICollection<IWriteablePlaceholderNodeState>.CopyTo(IWriteablePlaceholderNodeState[] array, int index) { CopyTo((IFramePlaceholderNodeState[])array, index); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IWriteablePlaceholderNodeState>.Remove(IWriteablePlaceholderNodeState item) { return Remove((IFramePlaceholderNodeState)item); }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new FramePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
