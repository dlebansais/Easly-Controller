#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IWriteableNodeStateList : IReadOnlyNodeStateList, IList<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        new IWriteableNodeState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteableNodeState> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    internal class WriteableNodeStateList : Collection<IWriteableNodeState>, IWriteableNodeStateList
    {
        #region ReadOnly
        IReadOnlyNodeState IReadOnlyNodeStateList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableNodeState)value; } }
        IReadOnlyNodeState IList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableNodeState)value; } }
        int IList<IReadOnlyNodeState>.IndexOf(IReadOnlyNodeState value) { return IndexOf((IWriteableNodeState)value); }
        void IList<IReadOnlyNodeState>.Insert(int index, IReadOnlyNodeState item) { Insert(index, (IWriteableNodeState)item); }
        void ICollection<IReadOnlyNodeState>.Add(IReadOnlyNodeState item) { Add((IWriteableNodeState)item); }
        bool ICollection<IReadOnlyNodeState>.Contains(IReadOnlyNodeState value) { return Contains((IWriteableNodeState)value); }
        void ICollection<IReadOnlyNodeState>.CopyTo(IReadOnlyNodeState[] array, int index) { CopyTo((IWriteableNodeState[])array, index); }
        bool ICollection<IReadOnlyNodeState>.IsReadOnly { get { return ((ICollection<IWriteableNodeState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyNodeState>.Remove(IReadOnlyNodeState item) { return Remove((IWriteableNodeState)item); }
        IEnumerator<IReadOnlyNodeState> IEnumerable<IReadOnlyNodeState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyNodeState IReadOnlyList<IReadOnlyNodeState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new WriteableNodeStateReadOnlyList(this);
        }
    }
}
