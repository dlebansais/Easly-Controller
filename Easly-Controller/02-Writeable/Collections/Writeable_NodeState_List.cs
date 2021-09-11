namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableNodeStateList : ReadOnlyNodeStateList, ICollection<IWriteableNodeState>, IEnumerable<IWriteableNodeState>, IList<IWriteableNodeState>, IReadOnlyCollection<IWriteableNodeState>, IReadOnlyList<IWriteableNodeState>
    {
        /// <inheritdoc/>
        public new IWriteableNodeState this[int index] { get { return (IWriteableNodeState)base[index]; } set { base[index] = value; } }

        #region IWriteableNodeState
        void ICollection<IWriteableNodeState>.Add(IWriteableNodeState item) { Add(item); }
        bool ICollection<IWriteableNodeState>.Contains(IWriteableNodeState item) { return Contains(item); }
        void ICollection<IWriteableNodeState>.CopyTo(IWriteableNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteableNodeState>.Remove(IWriteableNodeState item) { return Remove(item); }
        bool ICollection<IWriteableNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyNodeState>)this).IsReadOnly; } }
        IEnumerator<IWriteableNodeState> IEnumerable<IWriteableNodeState>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IWriteableNodeState)iterator.Current; } }
        IWriteableNodeState IList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } set { this[index] = value; } }
        int IList<IWriteableNodeState>.IndexOf(IWriteableNodeState item) { return IndexOf(item); }
        void IList<IWriteableNodeState>.Insert(int index, IWriteableNodeState item) { Insert(index, item); }
        IWriteableNodeState IReadOnlyList<IWriteableNodeState>.this[int index] { get { return (IWriteableNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new WriteableNodeStateReadOnlyList(this);
        }
    }
}
