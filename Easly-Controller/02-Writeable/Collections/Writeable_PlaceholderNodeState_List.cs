#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateList : IReadOnlyPlaceholderNodeStateList, IList<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new IWriteablePlaceholderNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class WriteablePlaceholderNodeStateList : Collection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateList
    {
        #region ReadOnly
        IReadOnlyPlaceholderNodeState IReadOnlyPlaceholderNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IWriteablePlaceholderNodeState)value; } }
        IReadOnlyPlaceholderNodeState IList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IWriteablePlaceholderNodeState)value; } }
        int IList<IReadOnlyPlaceholderNodeState>.IndexOf(IReadOnlyPlaceholderNodeState value) { return IndexOf((IWriteablePlaceholderNodeState)value); }
        void IList<IReadOnlyPlaceholderNodeState>.Insert(int index, IReadOnlyPlaceholderNodeState item) { Insert(index, (IWriteablePlaceholderNodeState)item); }
        void ICollection<IReadOnlyPlaceholderNodeState>.Add(IReadOnlyPlaceholderNodeState item) { Add((IWriteablePlaceholderNodeState)item); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Contains(IReadOnlyPlaceholderNodeState value) { return Contains((IWriteablePlaceholderNodeState)value); }
        void ICollection<IReadOnlyPlaceholderNodeState>.CopyTo(IReadOnlyPlaceholderNodeState[] array, int index) { CopyTo((IWriteablePlaceholderNodeState[])array, index); }
        bool ICollection<IReadOnlyPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IWriteablePlaceholderNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyPlaceholderNodeState>.Remove(IReadOnlyPlaceholderNodeState item) { return Remove((IWriteablePlaceholderNodeState)item); }
        IEnumerator<IReadOnlyPlaceholderNodeState> IEnumerable<IReadOnlyPlaceholderNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyPlaceholderNodeState IReadOnlyList<IReadOnlyPlaceholderNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new WriteablePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
