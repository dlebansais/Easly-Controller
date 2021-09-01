#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public class WriteablePlaceholderNodeStateList : ReadOnlyPlaceholderNodeStateList, ICollection<IWriteablePlaceholderNodeState>, IEnumerable<IWriteablePlaceholderNodeState>, IList<IWriteablePlaceholderNodeState>, IReadOnlyCollection<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        #region IWriteablePlaceholderNodeState
        void ICollection<IWriteablePlaceholderNodeState>.Add(IWriteablePlaceholderNodeState item) { Add(item); }
        bool ICollection<IWriteablePlaceholderNodeState>.Contains(IWriteablePlaceholderNodeState item) { return Contains(item); }
        void ICollection<IWriteablePlaceholderNodeState>.CopyTo(IWriteablePlaceholderNodeState[] array, int arrayIndex) { for (int i = 0; i < Count; i++) array[arrayIndex + i] = (IWriteablePlaceholderNodeState)this[i]; }
        bool ICollection<IWriteablePlaceholderNodeState>.Remove(IWriteablePlaceholderNodeState item) { return Remove(item); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return false; } }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return new List<IWriteablePlaceholderNodeState>(this).GetEnumerator(); }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<IWriteablePlaceholderNodeState>.IndexOf(IWriteablePlaceholderNodeState item) { return IndexOf(item); }
        void IList<IWriteablePlaceholderNodeState>.Insert(int index, IWriteablePlaceholderNodeState item) { Insert(index, item); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } }
        #endregion

        public override ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new WriteablePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
