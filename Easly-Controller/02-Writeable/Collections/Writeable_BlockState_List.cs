namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteableBlockStateList : ReadOnlyBlockStateList, ICollection<IWriteableBlockState>, IEnumerable<IWriteableBlockState>, IList<IWriteableBlockState>, IReadOnlyCollection<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        #region IWriteableBlockState
        void ICollection<IWriteableBlockState>.Add(IWriteableBlockState item) { Add(item); }
        bool ICollection<IWriteableBlockState>.Contains(IWriteableBlockState item) { return Contains(item); }
        void ICollection<IWriteableBlockState>.CopyTo(IWriteableBlockState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteableBlockState>.Remove(IWriteableBlockState item) { return Remove(item); }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IReadOnlyBlockState>)this).IsReadOnly; } }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return ((IList<IWriteableBlockState>)this).GetEnumerator(); }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return (IWriteableBlockState)this[index]; } set { this[index] = value; } }
        int IList<IWriteableBlockState>.IndexOf(IWriteableBlockState item) { return IndexOf(item); }
        void IList<IWriteableBlockState>.Insert(int index, IWriteableBlockState item) { Insert(index, item); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return (IWriteableBlockState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new WriteableBlockStateReadOnlyList(this);
        }
    }
}
