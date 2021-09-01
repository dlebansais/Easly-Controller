#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    internal class WriteableNodeStateList : ReadOnlyNodeStateList, ICollection<IWriteableNodeState>, IEnumerable<IWriteableNodeState>, IList<IWriteableNodeState>, IReadOnlyCollection<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        #region IWriteableNodeState
        void ICollection<IWriteableNodeState>.Add(IWriteableNodeState item) { Add(item); }
        bool ICollection<IWriteableNodeState>.Contains(IWriteableNodeState item) { return Contains(item); }
        void ICollection<IWriteableNodeState>.CopyTo(IWriteableNodeState[] array, int arrayIndex) { for (int i = 0; i < Count; i++) array[arrayIndex + i] = (IWriteableNodeState)this[i]; }
        bool ICollection<IWriteableNodeState>.Remove(IWriteableNodeState item) { return Remove(item); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return false; } }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { return new List<IWriteableNodeState>(this).GetEnumerator(); }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } set { this[index] = value; } }
        int IList<IWriteableNodeState>.IndexOf(IWriteableNodeState item) { return IndexOf(item); }
        void IList<IWriteableNodeState>.Insert(int index, IWriteableNodeState item) { Insert(index, item); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion

        public override ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new WriteableNodeStateReadOnlyList(this);
        }
    }
}
